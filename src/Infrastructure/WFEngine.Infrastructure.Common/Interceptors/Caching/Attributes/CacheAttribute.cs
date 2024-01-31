namespace WFEngine.Infrastructure.Common.Interceptors.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CacheAttribute : Attribute
    {
        public string Key { get;  set; }
        public bool AddNullValue { get; set; }

        public CacheAttribute(string key, bool addNullValue = false)
        {
            Key = key;
            AddNullValue = addNullValue;
        }
    }
}
