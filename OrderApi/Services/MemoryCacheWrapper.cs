using Microsoft.Extensions.Caching.Memory;
public class MemoryCacheWrapper : IMemoryCacheWrapper
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheWrapper(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
    {
        _memoryCache.Set(key, value, options);
    }

    public bool TryGetValue<TItem>(object key, out TItem value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public void Remove(object key)
    {
        _memoryCache.Remove(key);
    }

    public void Dispose()
    {
        _memoryCache.Dispose();
    }
}
    