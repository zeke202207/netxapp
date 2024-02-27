using System.Collections.Generic;

namespace NetX.AppContainer.Common
{
    public static class ApplicationExtentions
    {
        public static T Get<T>(this IDictionary<string, object> cache, string key)
        {
            if (cache.TryGetValue(key, out var value))
                return (T)value;
            return default(T);
        }

        public static void Set(this IDictionary<string, object> cache, string key, object value)
        {
            if (cache.ContainsKey(key))
                cache[key] = value;
            else
                cache.Add(key, value);
        }
    }
}
