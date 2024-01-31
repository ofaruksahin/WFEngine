using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WFEngine.Application.Common.Extensions;

namespace WFEngine.Infrastructure.Common.Interceptors.Extensions
{
    public static class InterceptorExtensions
    {
        public static IServiceCollection AddRepositoryInterceptors(this IServiceCollection @this, IConfiguration configuration)
        {
            @this.AddSingleton(new ProxyGenerator());

            var interceptorOptions = configuration.GetOptions<CastleInterceptorOptions>();

            var types = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Select(assembly => assembly.GetTypes())
                .SelectMany(types => types)
                .ToList();

            foreach (var interceptor in interceptorOptions.Interceptors)
            {
                Type interceptorType = types.FirstOrDefault(type => type.Name.Contains(interceptor));
                @this.AddTransient(typeof(IAsyncInterceptor), interceptorType);
            }

            return @this;
        }
    }
}
