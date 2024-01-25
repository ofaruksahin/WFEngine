using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Domain.Common.ValueObjects
{
    public class InternationalizationOptions : IOptions
    {
        public string Key => "WFEngine:Options:Internationalization";

        public string DefaultCulture { get; set; }
        public string[] SupportedCultures { get; set; }

        public InternationalizationOptions()
        {
            SupportedCultures = new string[0];
        }
    }
}
