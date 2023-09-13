
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Application.Interfaces.Commons;
public interface IConfigService
{
    void AddServices(IServiceCollection services, IConfiguration configuration);
    void AddAppBuilder(IApplicationBuilder applications);
    int Order { get; }
}
