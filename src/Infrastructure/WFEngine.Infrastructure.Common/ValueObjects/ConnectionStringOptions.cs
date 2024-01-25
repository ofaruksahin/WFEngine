using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Infrastructure.Common.ValueObjects
{
    public class ConnectionStringOptions : IOptions
    {
        public string Key => "ConnectionStrings";
        public string AuthorizationConfigurationDbContext { get; set; }
    }
}
