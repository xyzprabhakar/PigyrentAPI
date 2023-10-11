﻿using Microsoft.Extensions.Caching.Memory;

namespace API.Classes
{
    public class SimpleMemoryCache
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public Titem GetOrCreate<Titem>(object key, Func<Titem> createItem)
        {
            Titem cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry!))// Look for cache key.
            {
                // Key not in cache, so get data.
                cacheEntry = createItem();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                // Save data in cache.
                _cache.Set(key, cacheEntry);
            }
            return cacheEntry;
        }
    }
}
