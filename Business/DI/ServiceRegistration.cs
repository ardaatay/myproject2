using Business.Interceptors;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Business.Abstract;

namespace Business.DI;

public static class ServiceRegistration
{
    public static void AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TImplementation>();
        services.AddScoped(typeof(TInterface), serviceProvider =>
        {
            var proxyGenerator = new ProxyGenerator();
            var actual = serviceProvider.GetRequiredService<TImplementation>();
            var logService = serviceProvider.GetRequiredService<ILogService>();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            
            var interceptors = new IInterceptor[]
            {
                new LogInterceptor(logService, httpContextAccessor),
                new ExceptionInterceptor()
            };
            
            return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(
                actual,
                new ProxyGenerationOptions { Selector = new InterceptorSelector(actual.GetType()) },
                interceptors);
        });
    }
}