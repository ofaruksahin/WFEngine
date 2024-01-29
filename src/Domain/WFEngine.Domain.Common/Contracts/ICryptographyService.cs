namespace WFEngine.Domain.Common.Contracts
{
    public interface ICryptographyService
    {
        string Encrypt(string text);
        string Decrypt(string encryptedText);
    }
}
