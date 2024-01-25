using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Domain.Common.ValueObjects
{
    public class LoggingOptions : IOptions
    {
        public string Key => "WFEngine:Options:Logging";
        public string ApplicationName { get; set; }
        public string LogDirectory { get; set; }
    }
}
