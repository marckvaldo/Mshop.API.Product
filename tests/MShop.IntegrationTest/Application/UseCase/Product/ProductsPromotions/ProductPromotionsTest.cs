using Microsoft.Extensions.Caching.Distributed;
using MShop.Application.UseCases.Product.ListProducts;
using useCaseProducts =  MShop.Application.UseCases.Product.Productspromotions;
using MShop.Business.Validation;
using MShop.Repository.Context;
using Cache = Mshop.Cache.RepositoryRedis;
using MShop.Repository.Repository;
using Mshop.Cache.RepositoryRedis;
using MShop.Business.Entity;
using MShop.Business.Exception;

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

        public ProductPromotionsTest()
        {
            _DbContext = CreateDBContext();
            _cacheMemory = CreateCache();
            _productRepository = new(_DbContext);
            _redisRepository = new(_cacheMemory);
        }

        [Fact(DisplayName = nameof(ProductionPromotions))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async void ProductionPromotions()
        {
            var notification = new Notifications();

            var productsFake = FakerList(20);
            await _DbContext.Products.AddRangeAsync(productsFake);
            await _DbContext.SaveChangesAsync();

            var useCase = new useCaseProducts.ProductsPromotions(_redisRepository, _productRepository, notification);
            var outPut = await useCase.Handle();

            Assert.NotNull(outPut);
            Assert.False(notification.HasErrors()); 

            foreach (var item in outPut)
            {
                var expectItem = productsFake.FirstOrDefault(p => p.Id == item.Id);
                Assert.NotNull(expectItem);
                Assert.Equal(expectItem.Name, item.Name);
                Assert.Equal(expectItem.Description, expectItem.Description);
                Assert.Equal(expectItem.Imagem, expectItem.Imagem);
                Assert.Equal(expectItem.Price, expectItem.Price);
                Assert.Equal(expectItem.Activate, expectItem.Activate);
            }
        }



        [Fact(DisplayName = nameof(sholdReturErrorWhenCatGetProductionPromotions))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task sholdReturErrorWhenCatGetProductionPromotions()
        {
            var notification = new Notifications();

            var useCase = new useCaseProducts.ProductsPromotions(_redisRepository, _productRepository, notification);
            var outPut = async () => await useCase.Handle();

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);
            Assert.False(notification.HasErrors());

        }
    }
}
