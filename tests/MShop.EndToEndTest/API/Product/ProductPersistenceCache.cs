using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.EndToEndTest.API.Product
{
    public class ProductPersistenceCache
    {
        private readonly IDistributedCache _distributedCache;

        public ProductPersistenceCache(IDistributedCache context)
        {
            _distributedCache = context;
        }

        public async Task DeleteKey(string key)
        {
            await _distributedCache.RemoveAsync(key.ToLower());
        }

    }
}
