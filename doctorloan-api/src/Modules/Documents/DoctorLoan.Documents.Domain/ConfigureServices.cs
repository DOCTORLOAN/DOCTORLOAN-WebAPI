using DoctorLoan.Application.Interfaces.Commons;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Documents.Infrastructure;

public class ConfigureServices : IConfigService
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<ICourseService, CourseService>();
    }
    public void AddAppBuilder(IApplicationBuilder applications)
    {

    }
    public int Order => 0;
}

