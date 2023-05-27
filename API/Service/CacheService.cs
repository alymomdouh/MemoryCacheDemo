using API.IService;
using Microsoft.Extensions.Caching.Memory;

namespace API.Service
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache; 
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        } 
        public T GetData<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public bool SetData<T>(string key, T value, MemoryCacheEntryOptions cacheEntryOptions)
        {
            bool res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Set(key, value, cacheEntryOptions);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }
        public void RemoveData(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Remove(key);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        } 
    }
}
