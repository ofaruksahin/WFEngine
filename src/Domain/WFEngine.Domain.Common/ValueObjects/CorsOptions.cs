using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Domain.Common.ValueObjects
{
    public class CorsOptions : IOptions
    {
        public string Key => "WFEngine:Options:Cors";

        public string[] AllowedOrigins { get; set; }

        public CorsOptions()
        {
            AllowedOrigins = new string[0];
        }
    }
}
