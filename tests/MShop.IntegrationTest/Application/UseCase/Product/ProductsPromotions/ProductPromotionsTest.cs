using Microsoft.Extensions.Caching.Distributed;
using Mshop.Cache.RepositoryRedis;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using useCaseProducts = MShop.Application.UseCases.Product.Productspromotions;
using MShop.Core.Message;
using MShop.Core.Exception;

namespace MShop.IntegrationTests.Application.UseCase.Product.ProductsPromotions
{
    [Collection("Repository Products Collection")]
    [CollectionDefinition("Repository Products Collection", DisableParallelization = true)]
    public class ProductPromotionsTest: ProductPromotionsTestFixture
    {
        private readonly ProductRepository _productRepository;
        private readonly IDistributedCache _cacheMemory;
        private readonly RepositoryDbContext _DbContext;
        private readonly RedisRepository _redisRepository;
        private readonly INotification _notification;
        private readonly ProductPersistence _productPersistence;


        public ProductPromotionsTest()
        {
            _DbContext = CreateDBContext();
            //_cacheMemory = CreateCache();
            _productRepository = new(_DbContext);
            //_redisRepository = new(_cacheMemory);
            _productPersistence = new ProductPersistence(_DbContext);
            _notification = new Notifications();
        }

        [Fact(DisplayName = nameof(ProductionPromotions))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async void ProductionPromotions()
        {
            /*
            //var notification = new Notifications();

            var productsFake = FakerList(20);
            await _productPersistence.CreateList(productsFake);
            //await _DbContext.Products.AddRangeAsync(productsFake);
            //await _DbContext.SaveChangesAsync();

            var useCase = new useCaseProducts.ProductsPromotions(_redisRepository, _productRepository, _notification);
            var outPut = await useCase.Handler();

            Assert.NotNull(outPut);
            Assert.False(_notification.HasErrors()); 

            foreach (var item in outPut)
            {
                var expectItem = productsFake.FirstOrDefault(p => p.Id == item.Id);
                Assert.NotNull(expectItem);
                Assert.Equal(expectItem.Name, item.Name);
                Assert.Equal(expectItem.Description, expectItem.Description);
                Assert.Equal(expectItem.Thumb, expectItem.Thumb);
                Assert.Equal(expectItem.Price, expectItem.Price);
                Assert.Equal(expectItem.Activate, expectItem.Activate);
            }*/
        }



        [Fact(DisplayName = nameof(sholdReturErrorWhenCatGetProductionPromotions))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task sholdReturErrorWhenCatGetProductionPromotions()
        {
            /*
            //var notification = new Notifications();

            var useCase = new useCaseProducts.ProductsPromotions(_redisRepository, _productRepository, _notification);
            var outPut = async () => await useCase.Handler();

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);
            Assert.False(_notification.HasErrors());
            */
        }
    }
}
