using Microsoft.Extensions.Caching.Memory;

namespace ReviewAPI.Services.Caching
{
    /// <exception cref="ArgumentStringEmptyException"/>
    public class Cache : ICache
    {
        private readonly IMemoryCacheWrapper _cache;

        public Cache(IMemoryCacheWrapper cache)
        {
            _cache = cache;
        }
        public void Add(CacheType type, int key, object value)
        {
            if (value == null) throw new ArgumentNullException("value is null");
            _cache.Set(
                type.ToString() + "_" + key,
                value,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3))
            );
        }
        public void Remove(CacheType type, int key)
        {
            _cache.Remove(type.ToString() + "_" + key);
        }

        public void Clear()
        {
            _cache.Dispose();
        }

        public object? Get(CacheType type, int key)
        {
            if (_cache.TryGetValue(type.ToString() + "_" + key, out object value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool IsCached(CacheType type, int key)
        {
            bool isInCache = _cache.TryGetValue<object>(type.ToString() + "_" + key, out object value);

            if (isInCache && value is bool boolValue)
                return boolValue;
            else
                return isInCache;
        }
    }
}
