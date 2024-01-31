using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WFEngine.Application.Common.Extensions;

namespace WFEngine.Infrastructure.Common.Caching
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection @this, IConfiguration configuration)
        {
            var cacheOptions = configuration.GetOptions<CacheOptions>();

            @this.AddSingleton<IDatabase>((serviceProvider) =>
            {
                return ConnectionMultiplexer
                    .Connect(cacheOptions.ConnectionString)
                    .GetDatabase(0);
            });

            return @this;
        }
    }
}
