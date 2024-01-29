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
                            @this.AddSingleton(firstInterface, type);
                        else
                            @this.AddSingleton(type);
                        break;
                    case ServiceLifetime.Scoped:
                        if (firstInterface is not null)
                            @this.AddScoped(firstInterface, type);
                        else
                            @this.AddScoped(type);
                        break;
                    case ServiceLifetime.Transient:
                        if (firstInterface is not null)
                            @this.AddTransient(firstInterface, type);
                        else
                            @this.AddTransient(type);
                        break;
                }
            }

            return @this;
        }
    }
}
