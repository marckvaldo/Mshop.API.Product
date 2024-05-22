using Microsoft.Extensions.Caching.Distributed;
using MShop.Core.Cache;
using System.Text.Json;

namespace Mshop.Cache.RepositoryRedis
{
    public class RedisRepository : ICacheRepository
    {
        private readonly IDistributedCache _distributedCache;

        public RedisRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new NotImplementedException(nameof(distributedCache));
        }

        public async Task<TResult?> GetKey<TResult>(string key)
        {
            var result = await _distributedCache.GetStringAsync(key.ToLower());
            if (string.IsNullOrEmpty(result)) return default;   
            return JsonSerializer.Deserialize<TResult?>(result);
        }

        public async Task<List<TResult?>?> GetKeyCollection<TResult>(string key)
        {
            var result = await _distributedCache.GetStringAsync(key.ToLower());
            if (string.IsNullOrEmpty(result)) return default;
            return JsonSerializer.Deserialize<List<TResult?>?>(result);
        }

        public async Task SetKey(string key, object value, TimeSpan TimeExpiration)
        {
            var options = new DistributedCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeExpiration);

            var newValue = JsonSerializer.Serialize(value); 
            await _distributedCache.SetStringAsync(key.ToLower(), newValue, options);
        }
        public async Task DeleteKey(string key)
        {
            await _distributedCache.RemoveAsync(key.ToLower()); 
        }

        public async Task SetKeyCollection(string key, object value, TimeSpan TimeExpiration)
        {
            var options = new DistributedCacheEntryOptions()
                      .SetAbsoluteExpiration(TimeExpiration);

            var newValue = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key.ToLower(), newValue, options);
        }

    }
}
