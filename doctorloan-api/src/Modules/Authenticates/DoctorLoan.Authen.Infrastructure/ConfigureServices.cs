using DoctorLoan.Application.Interfaces.Commons;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Authen.Infrastructure;
public class ConfigureServices : IConfigService
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {

    }
    public void AddAppBuilder(IApplicationBuilder applications)
    {

    }

    public int Order => 1;
}