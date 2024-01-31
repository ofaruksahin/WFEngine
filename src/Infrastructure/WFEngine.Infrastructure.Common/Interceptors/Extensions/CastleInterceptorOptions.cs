using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Infrastructure.Common.Interceptors.Extensions
{
    public class CastleInterceptorOptions : IOptions
    {
        public string Key => "WFEngine:Options:Castle:Interceptors";

        public string[] Interceptors { get; set; }

        public CastleInterceptorOptions()
        {
            Interceptors = new string[0];
        }
    }
}
