using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Infrastructure.Common.Caching
{
    public class CacheOptions : IOptions
    {
        public string Key => "WFEngine:Options:Cache";

        public string ConnectionString { get; set; }
    }
}
