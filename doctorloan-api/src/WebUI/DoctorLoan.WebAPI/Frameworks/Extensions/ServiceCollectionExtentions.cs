using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Settings;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Interfaces;
using DoctorLoan.Infrastructure.Persistence;
using DoctorLoan.WebAPI.Filters;
using DoctorLoan.WebAPI.Frameworks.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace DoctorLoan.WebAPI.Frameworks.Extensions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddWebApplicationServices(this IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder, bool isDevelopment)
    {
        services.AddSingleton(typeFinder);
        services.AddHttpContextAccessor();
        services.AddApplicationDatabase(configuration);
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ICurrentRequestInfoService, CurrentRequestInfoService>();
        services.AddTransient<ICurrentTranslateService, CurrentTranslateService>();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
        })
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "Doctor Loan API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.UseCamelCasing(true);
                });

        var jwtOption = configuration.GetSection(nameof(JWTTokenConfiguration));
        var jwtKey = jwtOption?.GetValue<string>(nameof(JWTTokenConfiguration.Key)) ?? string.Empty;
        var jwtAudience = jwtOption?.GetValue<string>(nameof(JWTTokenConfiguration.Audience)) ?? string.Empty;
        var jwtIssuer = jwtOption?.GetValue<string>(nameof(JWTTokenConfiguration.Issuer)) ?? string.Empty;
        services.AddAuthentication(k =>
        {
            k.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            k.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            k.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(p =>
        {
            var key = Encoding.UTF8.GetBytes(jwtKey);
            p.SaveToken = true;
            p.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,// on production make it true
                ValidateAudience = false,// on production make it true
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
            p.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    // Ensure we always have an error and error description.
                    if (string.IsNullOrEmpty(context.Error))
                        context.Error = "invalid_token";
                    if (string.IsNullOrEmpty(context.ErrorDescription))
                        context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                    // Add some extra context for expired tokens.
                    if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                        context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                        context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                    }

                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = context.Error,
                        error_description = context.ErrorDescription
                    }));
                },
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });

        //services.AddAuthorization(o =>
        //{
        //    o.DefaultPolicy = new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .RequireClaim("username", "true")
        //        .Build();

        //    var tradieAuthorizerPolicy = new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .RequireClaim("trade_license_number")
        //        .Build();
        //    o.AddPolicy("TradieOnly", tradieAuthorizerPolicy);

        //});

        var assemblies = typeFinder.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (!string.IsNullOrEmpty(assembly?.FullName) && !assembly.FullName.StartsWith("DoctorLoan"))
                continue;
            try
            {
                services.AddAutoMapper(assembly);
                services.AddValidatorsFromAssembly(assembly);
                //services.AddMediatR(assembly);

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        services.AddCustomResourceLocalization();
        services.AddAllConfigureServices(configuration, typeFinder);
        services.AddSettings(typeFinder);
        services.AddEnableCORS(configuration);

        if (!isDevelopment)
        {
            #region setup log to datadog

            var logConfig = configuration.GetSection(nameof(SerilogConfiguration));
            var serilogURL = logConfig.GetValue<string>("URL");
            var levelSystem = logConfig.GetValue<int>("MinimumLevel_System");
            var levelMicro = logConfig.GetValue<int>("MinimumLevel_Microsoft");
            var levelAsp = logConfig.GetValue<int>("MinimumLevel_Microsoft_AspNetCore");
            var apiKey = logConfig.GetValue<string>("ApiKey");
            var source = logConfig.GetValue<string>("Source");


         

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("System", (LogEventLevel)levelSystem)
                .MinimumLevel.Override("Microsoft", (LogEventLevel)levelMicro)
                .MinimumLevel.Override("Microsoft.AspNetCore", (LogEventLevel)levelAsp)
                .Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                
                .CreateLogger();

            #endregion
        }




        return services;

    }
    private static IServiceCollection AddAllConfigureServices(this IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder)
    {
        // add type finder

        // Add services to the container.
        var configServices = typeFinder.FindClassesOfType(typeof(IConfigService))
         .Select(service => (IConfigService)Activator.CreateInstance(service))
                .OrderByDescending(service => service.Order);
        foreach (var configService in configServices)
        {
            configService.AddServices(services, configuration);
        }
        return services;
    }

    private static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("DoctorLoan.UsersDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        return services;
    }

    private static IServiceCollection AddCustomResourceLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("vi-VN")
            };

            options.DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
            {
                var languages = context.Request.Headers["Accept-Language"].ToString();
                var currentLanguage = languages.Split(',').FirstOrDefault();
                var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "vi-VN" : currentLanguage;

                if (defaultLanguage != "vi" && defaultLanguage != "en-US")
                {
                    defaultLanguage = "vi-VN";
                }

                return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
            }));
        });
        return services;
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, ITypeFinder typeFinder)
    {
        var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
        foreach (var setting in settings)
        {
            services.AddTransient(setting,
                context => context.GetRequiredService<ISettingBaseService>().LoadSettingAsync(setting).Result);
        }
        return services;
    }
    private static void AddEnableCORS(this IServiceCollection services, IConfiguration _configuration)
    {
        var appSettings = _configuration.GetSection("SystemConfiguration").Get<SystemConfiguration>();
        var cors = appSettings.GetAllowCORS();
        if (cors.Length > 0)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins(cors)
                    .AllowAnyMethod()
                    .WithExposedHeaders("Content-Disposition", "content-disposition"));
            });

        }
    }
}
