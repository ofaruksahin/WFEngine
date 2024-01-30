using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WFEngine.Infrastructure.Common.IoC.Attributes;

namespace WFEngine.Infrastructure.Common.IoC.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection InjectServices(this IServiceCollection @this)
        {
            var types = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Select(assembly =>
                    assembly
                        .GetTypes()
                        .Where(type => type.GetCustomAttribute(typeof(InjectAttribute)) is not null)
                        .Select(type => type))
                .SelectMany(type => type);

            foreach (var type in types)
            {
                var injectAttribute = (InjectAttribute)type.GetCustomAttribute(typeof(InjectAttribute));

                if (injectAttribute is null) continue;

                var firstInterface = type
                    .GetInterfaces()
                    .Where(i => !i.IsGenericType)
                    .FirstOrDefault();

                switch (injectAttribute.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (firstInterface is not null)
                        {
                            if (injectAttribute.UseWithInterceptors)
                            {
                                @this.AddProxiedSingleton(firstInterface, type);
                            }
                            else
                            {
                                @this.AddSingleton(firstInterface, type);
                            }
                        }
                        else
                        {
                            @this.AddSingleton(type);
                        }
                        break;
                    case ServiceLifetime.Scoped:
                        if (firstInterface is not null)
                        {
                            if (injectAttribute.UseWithInterceptors)
                            {
                                @this.AddProxiedScoped(firstInterface, type);
                            }
                            else
                            {
                                @this.AddScoped(firstInterface, type);
                            }
                        }
                        else
                        {
                            @this.AddScoped(type);
                        }
                        break;
                    case ServiceLifetime.Transient:
                        if (firstInterface is not null)
                        {
                            if (injectAttribute.UseWithInterceptors)
                            {
                                @this.AddProxiedTransient(firstInterface, type);
                            }
                            else
                            {
                                @this.AddTransient(firstInterface, type);
                            }
                        }
                        else
                        {
                            @this.AddTransient(type);
                        }
                        break;
                }
            }

            return @this;
        }

        internal static void AddProxiedSingleton(this IServiceCollection services, Type typeInterface, Type typeImplementation)
        {
            services.AddSingleton(typeImplementation);
            services.AddSingleton(typeInterface, serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService(typeImplementation);
                var interceptors = serviceProvider.GetServices<IAsyncInterceptor>().ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(typeInterface, actual, interceptors);
            });
        }

        internal static void AddProxiedScoped(this IServiceCollection services, Type typeInterface, Type typeImplementation)
        {
            services.AddScoped(typeImplementation);
            services.AddScoped(typeInterface, serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService(typeImplementation);
                var interceptors = serviceProvider.GetServices<IAsyncInterceptor>().ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(typeInterface, actual, interceptors);
            });
        }

        internal static void AddProxiedTransient(this IServiceCollection services, Type typeInterface, Type typeImplementation)
        {
            services.AddTransient(typeImplementation);
            services.AddTransient(typeInterface, serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService(typeImplementation);
                var interceptors = serviceProvider.GetServices<IAsyncInterceptor>().ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(typeInterface, actual, interceptors);
            });
        }
    }
}
