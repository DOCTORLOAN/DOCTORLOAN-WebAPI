using DoctorLoan.Application.Common.Behaviours;
using DoctorLoan.Application.Features.Email;
using DoctorLoan.Application.Interfaces.Authenticates;
using DoctorLoan.Application.Interfaces.Caching;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Application.Interfaces.Settings;
using DoctorLoan.Application.Interfaces.Users;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Infrastructure.Persistence.Interceptors;
using DoctorLoan.Infrastructure.Services;
using DoctorLoan.Infrastructure.Services.Authenticates;
using DoctorLoan.Infrastructure.Services.Caching;
using DoctorLoan.Infrastructure.Services.Commons;
using DoctorLoan.Infrastructure.Services.Medias;
using DoctorLoan.Infrastructure.Services.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Infrastructure;
public class ConfigureServices : IConfigService
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddHttpClient();

        services.Configure<SystemConfiguration>(configuration.GetSection(nameof(SystemConfiguration)));
        services.Configure<StorageConfiguration>(configuration.GetSection(nameof(StorageConfiguration)));
        services.Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)));
        services.Configure<SerilogConfiguration>(configuration.GetSection(nameof(SerilogConfiguration)));
        services.Configure<JWTTokenConfiguration>(configuration.GetSection(nameof(JWTTokenConfiguration)));

        services.AddScoped<IWebHelper, WebHelper>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IEmailSenderService, EmailSenderServices>();
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddTransient<IJWTServices, JWTServices>();


        services.AddTransient<ILocker, MemoryCacheManager>();
        services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        services.AddScoped<ISettingBaseService, SettingBaseService>();
        services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
        services.AddScoped<IUserBaseService, UserBaseService>();
        services.AddScoped<IMediaService, PhysicalStoreMediaService>();
    }

    public void AddAppBuilder(IApplicationBuilder applications)
    {

    }

    public int Order => 9999;
}
