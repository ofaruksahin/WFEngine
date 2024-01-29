using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using WFEngine.Domain.Common.Contracts;
using WFEngine.Infrastructure.Common.IoC.Attributes;

namespace WFEngine.Infrastructure.Common.Cryptography
{
    [Inject(ServiceLifetime.Singleton)]
    public class CryptographyService : ICryptographyService
    {
        private readonly CryptographyOptions _options;

        public CryptographyService(CryptographyOptions options)
        {
            _options = options;
        }

        private byte[] ToSalt(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public string Encrypt(string text)
        {
            using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(text, ToSalt(_options.EncryptionKey), 10000, HashAlgorithmName.SHA256))
            {
                byte[] key = derivedBytes.GetBytes(32);
                byte[] iv = derivedBytes.GetBytes(16);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(encryptedText, ToSalt(_options.EncryptionKey), 10000, HashAlgorithmName.SHA256))
            {
                byte[] key = derivedBytes.GetBytes(32);
                byte[] iv = derivedBytes.GetBytes(16); 

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
