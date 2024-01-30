using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace WFEngine.Infrastructure.Common.Interceptors.Extensions
{
    public static class InterceptorExtensions
    {
        public static IServiceCollection AddRepositoryInterceptors(this IServiceCollection @this)
        {
            @this.AddSingleton(new ProxyGenerator());

            var interceptors = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Select(assembly =>
                    assembly.GetTypes())
                .Select(types => 
                    types.Where(type => 
                        type.BaseType != null && type.BaseType == typeof(AsyncInterceptorBase)))
                .SelectMany(type => type)
                .Select(type => type);

            foreach (var interceptor in interceptors)
                @this.AddTransient(typeof(IAsyncInterceptor), interceptor);

            return @this;
        }
    }
}
