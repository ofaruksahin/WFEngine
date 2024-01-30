namespace WFEngine.Domain.Common.Contracts
{
    public interface ICache
    {
        Task<bool> Any(string key);
        Task<T> Get<T>(string key);
        Task Set(string key, object value);
    }
}
