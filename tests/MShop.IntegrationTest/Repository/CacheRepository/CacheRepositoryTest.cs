using Microsoft.Extensions.Caching.Distributed;
using Mshop.Cache.RepositoryRedis;
using MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache = Mshop.Cache.RepositoryRedis;

namespace MShop.IntegrationTests.Repository.CacheRepository
{
    [Collection("Repository Products Collection")]
    [CollectionDefinition("Repository Products Collection", DisableParallelization = true)]
    public class CacheRepositoryTest: CacheRepositoryTestFixture
    {
        private readonly Cache.RedisRepository _redisRepository;
        private readonly PersistenceCache _persistenceCache;
        private readonly IDistributedCache _cacheMemory;

        public CacheRepositoryTest()
        {
            
            _cacheMemory = CreateCache();
            _redisRepository = new(_cacheMemory);
            _persistenceCache = new(_cacheMemory);
        }

        [Fact(DisplayName = nameof(GetKey))]
        [Trait("integration-infra.Cache", "Chace")]

        public void GetKey()
        {
            var product = Faker();

            _persistenceCache.SetKey("getProduct", product);
            var cache = _redisRepository.GetKey<Product>("getProduct");

            Assert.NotNull(cache.Result);
            Assert.Equal(product.Name, cache.Result.Name);
            Assert.Equal(product.Description,cache.Result.Description);
            Assert.Equal(product.Imagem, cache.Result.Imagem);  
            Assert.Equal(product.Price, cache.Result.Price);
            Assert.Equal(product.Id, cache.Result.Id);  
            
        }


        [Fact(DisplayName = nameof(SetKey))]
        [Trait("integration-infra.Cache", "Chace")]
        public async void SetKey()
        {
            var product = Faker();

            await _redisRepository.SetKey("setProduct", product);
            var cache = _persistenceCache.GetKey<Product>("setProduct");

            Assert.NotNull(cache.Result);
            Assert.Equal(product.Name, cache.Result.Name);
            Assert.Equal(product.Description, cache.Result.Description);
            Assert.Equal(product.Imagem, cache.Result.Imagem);
            Assert.Equal(product.Price, cache.Result.Price);
            Assert.Equal(product.Id, cache.Result.Id);
        }




        [Fact(DisplayName = nameof(GetCollection))]
        [Trait("integration-infra.Cache", "Chace")]
        public async void GetCollection()
        {
            var products = FakerList();

            await _persistenceCache.SetKeyCollection("getCollectionProduct", products);
            var outPutCache = _redisRepository.GetKeyCollection<Product>("getCollectionProduct");

            Assert.NotNull(outPutCache.Result);

            foreach(var Item in outPutCache.Result)
            {
                Assert.NotNull(Item);
                var product = products.Where(p => p.Id == Item.Id).FirstOrDefault();

                Assert.NotNull(product);
                Assert.Equal(product.Name, Item.Name);
                Assert.Equal(product.Description, Item.Description);
                Assert.Equal(product.Imagem, Item.Imagem);
                Assert.Equal(product.Price, Item.Price);
                Assert.Equal(product.Id, Item.Id);
                Assert.Equal(product.Stock, Item.Stock);
            }
           
        }



        [Fact(DisplayName = nameof(SetCollection))]
        [Trait("integration-infra.Cache", "Chace")]
        public async void SetCollection()
        {
            var products = FakerList();

            await _redisRepository.SetKeyCollection("setCollectionProduct", products);
            var outPutCache = _persistenceCache.GetKeyCollection<Product>("setCollectionProduct");

            Assert.NotNull(outPutCache.Result);

            foreach (var Item in outPutCache.Result)
            {
                Assert.NotNull(Item);
                var product = products.Where(p => p.Id == Item.Id).FirstOrDefault();

                Assert.NotNull(product);
                Assert.Equal(product.Name, Item.Name);
                Assert.Equal(product.Description, Item.Description);
                Assert.Equal(product.Imagem, Item.Imagem);
                Assert.Equal(product.Price, Item.Price);
                Assert.Equal(product.Id, Item.Id);
                Assert.Equal(product.Stock, Item.Stock);
            }

        }



        [Fact(DisplayName = nameof(DeleteCollection))]
        [Trait("integration-infra.Cache", "Chace")]
        public async void DeleteCollection()
        {
            var products = FakerList();

            await _persistenceCache.SetKeyCollection("setCollectionProduct", products);
            await _redisRepository.DeleteKey("setCollectionProduct");
            var outPutCache = _persistenceCache.GetKeyCollection<Product>("setCollectionProduct");

            Assert.Null(outPutCache.Result);

        }



        [Fact(DisplayName = nameof(DeleteKey))]
        [Trait("integration-infra.Cache", "Chace")]
        public async void DeleteKey()
        {
            var product = Faker();

            await _persistenceCache.SetKey("DeleteKey", product);
            await _redisRepository.DeleteKey("DeleteKey");
            var outPutCache = _persistenceCache.GetKey<Product>("DeleteKey");

            Assert.Null(outPutCache.Result);

        }



        [Fact(DisplayName = nameof(GetKeyEmpty))]
        [Trait("integration-infra.Cache", "Chace")]
        public async void GetKeyEmpty()
        {
 
            await _redisRepository.GetKey<Product>("keyEmpty");
            var outPutCache = _persistenceCache.GetKey<Product>("keyEmpty");

            Assert.Null(outPutCache.Result);

        }


    }
}
