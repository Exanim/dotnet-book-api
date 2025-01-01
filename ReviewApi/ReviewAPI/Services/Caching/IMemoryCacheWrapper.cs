using Microsoft.Extensions.Caching.Memory;

namespace ReviewAPI.Services.Caching
{
    public interface IMemoryCacheWrapper
    {
        void Dispose();
        void Remove(object key);
        void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options);
        bool TryGetValue<TItem>(object key, out TItem value);
    }
}
