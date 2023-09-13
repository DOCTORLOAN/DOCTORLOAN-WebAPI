using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Application;
public class EngineContext
{
    private static IServiceProvider _serviceProvider;
    public static void Init(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected static IServiceProvider GetServiceProvider()
    {
        if (_serviceProvider == null)
            return null;
        var accessor = _serviceProvider?.GetService<IHttpContextAccessor>();
        var context = accessor?.HttpContext;
        return context?.RequestServices ?? _serviceProvider;
    }
    public static T GetService<T>() where T : class
    {
        var sp = GetServiceProvider();
        if (sp == null)
            return null;
        return (T)sp.GetService(typeof(T));
    }
    public static T GetService<T>(IServiceProvider provider) where T : class
    {

        if (provider == null)
            return null;
        return (T)provider.GetService(typeof(T));
    }
    public static object GetService(Type type)
    {
        var sp = GetServiceProvider();
        if (sp == null)
            return null;
        return sp.GetService(type);
    }
    public static IEnumerable<T> GetServices<T>() where T : class
    {
        var sp = GetServiceProvider();
        if (sp == null)
            return null;
        return sp.GetServices(typeof(T)) as IEnumerable<T>;
    }


}