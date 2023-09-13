using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.WebAPI.Middlewares;
using Microsoft.Extensions.Options;

namespace DoctorLoan.WebAPI.Frameworks.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigWebApplication(this WebApplication app, ITypeFinder typeFinder)
    {
        app.UseLocalization();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {

            app.UseDeveloperExceptionPage();

            // app.UseMigrationsEndPoint();

            // Initialise and seed database
            //using (var scope = app.Services.CreateScope())
            //{
            //    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            //    await initialiser.InitialiseAsync();
            //    await initialiser.SeedAsync();
            //}
        }
        else
        {
            app.UseHsts();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseRequestResponseLogging();
        }

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


        app.UseSwaggerUi3(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.UseRouting();
        app.UseAllowCORS();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "api/{area:exists}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.MapFallbackToFile("index.html");
        ConfigureAllApp(app, typeFinder);
        EngineContext.Init(app.Services);

        return app;
    }
    private static void ConfigureAllApp(this IApplicationBuilder app, ITypeFinder typeFinder)
    {
        var configServices = typeFinder.FindClassesOfType(typeof(IConfigService))
         .Select(service => (IConfigService)Activator.CreateInstance(service))
                .OrderByDescending(service => service.Order);
        foreach (var configService in configServices)
        {
            configService.AddAppBuilder(app);
        }
    }
    private static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "vi-VN", "en-US" };
        var localizationOptions =
            new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        return app.UseRequestLocalization(localizationOptions);
    }
    private static IApplicationBuilder UseAllowCORS(this IApplicationBuilder app)
    {
        var setting = app.ApplicationServices.GetService(typeof(IOptions<SystemConfiguration>)) as IOptions<SystemConfiguration>;
        var allowCORSUrls = setting.Value.GetAllowCORS();
        if (allowCORSUrls.Length > 0)
        {
            app.UseCors(c =>
            {
                c.WithOrigins(allowCORSUrls)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed(origin => true)
                          .AllowCredentials();
            });
        }
        return app;
    }
}
