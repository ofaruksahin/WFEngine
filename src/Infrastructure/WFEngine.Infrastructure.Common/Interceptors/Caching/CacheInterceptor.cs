using Castle.DynamicProxy;
using System.Reflection;
using WFEngine.Domain.Common.Contracts;
using WFEngine.Infrastructure.Common.Interceptors.Caching.Attributes;

namespace WFEngine.Infrastructure.Common.Interceptors.Caching
{
    public class CacheInterceptor : AsyncInterceptorBase
    {
        private readonly ICache _cache;

        public CacheInterceptor(ICache cache)
        {
            _cache = cache;
        }

        protected override Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            return InterceptAsync(invocation, proceedInfo, proceed);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var cacheAttributes = (CacheAttribute[])invocation.MethodInvocationTarget.GetCustomAttributes(typeof(CacheAttribute));

            TResult returnValue = default(TResult);

            if (!cacheAttributes.Any())
            {
                returnValue = await proceed(invocation, proceedInfo).ConfigureAwait(false);

                return returnValue;
            }

            var cacheKeys = new List<CacheAttribute>();

            var methodParameters = invocation.Method.GetParameters();

            foreach (var cacheAttribute in cacheAttributes)
            {
                string key = cacheAttribute.Key;

                int index = -1;

                foreach (var item in invocation.Method.GetParameters())
                {
                    index++;

                    var argumentValue = invocation.Arguments[index];

                    if (ReplaceExpressions(ref key, item.Name, argumentValue))
                        continue;

                    if (!item.ParameterType.Namespace.StartsWith("System"))
                    {
                        var properties = argumentValue.GetType().GetProperties();

                        foreach (var property in properties)
                        {
                            var propertyName = property.Name;
                            var propertyValue = property.GetValue(argumentValue);

                            ReplaceExpressions(ref key, propertyName, propertyValue);
                        }
                    }
                }

                cacheAttribute.Key = key;

                cacheKeys.Add(cacheAttribute);
            }

            IDictionary<string, bool> keyExists = new Dictionary<string, bool>();
            var cacheExistsTasks = new List<Task>();
            var cacheAddTasks = new List<Task>();

            foreach (var cacheKey in cacheKeys)
            {
                var task = Task.Run(async () =>
                {
                    var exists = await _cache.Any(cacheKey.Key);

                    keyExists.TryAdd(cacheKey.Key, exists);

                    return Task.CompletedTask;
                });

                cacheExistsTasks.Add(task);
            }

            await Task.WhenAll(cacheExistsTasks);

            if (keyExists.Values.Any(exists => exists))
            {
                var firstCacheKey = keyExists
                    .Where(keyValue => keyValue.Value)
                    .FirstOrDefault();

                returnValue = await _cache.Get<TResult>(firstCacheKey.Key);
            }
            else
            {
                returnValue = await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }

            foreach (var cacheAttribute in cacheAttributes)
            {
                if (!cacheAttribute.AddNullValue)
                {
                    if (returnValue is null) continue;
                    if (returnValue is IEnumerable<object> enumerable && !enumerable.Any()) continue;
                }

                if (keyExists.TryGetValue(cacheAttribute.Key, out var hasExists))
                {
                    if (!hasExists)
                    {
                        var task = Task.Run(async () =>
                        {
                            await _cache.Set(cacheAttribute.Key, returnValue);
                            return Task.CompletedTask;
                        });

                        cacheAddTasks.Add(task);
                    }
                }
            }

            await Task.WhenAll(cacheAddTasks);

            return returnValue;
        }

        private bool ReplaceExpressions(ref string key, string argumentName, object argumentValue)
        {
            var keyParam = string.Concat("{", string.Format("{0}", argumentName), "}");

            if (key.Contains(keyParam, StringComparison.InvariantCultureIgnoreCase))
            {
                key = key.Replace(keyParam, argumentValue.ToString(), StringComparison.InvariantCultureIgnoreCase);
                return true;
            }

            return false;
        }
    }
}
