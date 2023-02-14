using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.CacheRepository
{
    public class PersistenceCache
    {
        private readonly IDistributedCache _distributedCache;
        public PersistenceCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }   

        public async Task SetKey(string key,object value)
        {
            var newValue = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key.ToLower(), newValue);
        }
        public async Task SetKeyCollection(string key, object value)
        {
            var newValue = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key.ToLower(), newValue);
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
    }
}
