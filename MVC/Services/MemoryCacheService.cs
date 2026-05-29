using Microsoft.Extensions.Caching.Memory;

namespace MVC.Services;

public interface ICacheService
{
    T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration = null);
}

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration = null)
    {
        if (!_memoryCache.TryGetValue(key, out T cachedItem))
        {
            cachedItem = createItem();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration)
                .SetPriority(CacheItemPriority.High);

            if (slidingExpiration.HasValue)
                options.SetSlidingExpiration(slidingExpiration.Value);

            _memoryCache.Set(key, cachedItem, options);
        }

        return cachedItem;
    }
} 