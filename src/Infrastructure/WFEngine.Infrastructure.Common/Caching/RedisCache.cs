using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using WFEngine.Domain.Common.Contracts;
using WFEngine.Domain.Common.JsonSerializer;
using WFEngine.Infrastructure.Common.IoC.Attributes;

namespace WFEngine.Infrastructure.Common.Caching
{
    [Inject(lifetime: ServiceLifetime.Singleton)]
    public class RedisCache : ICache
    {
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RedisCache(IDatabase database)
        {
            _database = database;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
        }

        public async Task<bool> Any(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task<T> Get<T>(string key)
        {
            var json = await _database.StringGetAsync(key);

            return await json.ToString().ToObject<T>();
        }

        public async Task Set(string key, object value)
        {
            string json = await value.ToJson();

            await _database.StringSetAsync(key, new RedisValue(json));
        }
    }
}
