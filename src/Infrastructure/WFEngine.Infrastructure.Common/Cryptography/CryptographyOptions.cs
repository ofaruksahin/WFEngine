using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Infrastructure.Common.Cryptography
{
    public class CryptographyOptions : IOptions
    {
        public string Key => "WFEngine:Options:Cryptography";

        public string EncryptionKey { get; set; }
    }
}
