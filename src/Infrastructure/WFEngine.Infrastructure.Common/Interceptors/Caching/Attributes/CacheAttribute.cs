namespace WFEngine.Infrastructure.Common.Interceptors.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CacheAttribute : Attribute
    {
        public string Key { get; private set; }
        public bool AddNullValue { get; private set; }

        public CacheAttribute(string key, bool addNullValue = false)
        {
            Key = key;
            AddNullValue = addNullValue;
        }
    }
}
