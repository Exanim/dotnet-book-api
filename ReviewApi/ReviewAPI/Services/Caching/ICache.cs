using System.Security.AccessControl;

namespace ReviewAPI.Services.Caching
{
    public interface ICache
    {
        void Add(CacheType type, int key, object data);
        void Clear();
        object? Get(CacheType type, int key);
        bool IsCached(CacheType type, int key);
        void Remove(CacheType type, int key);
    }
}
