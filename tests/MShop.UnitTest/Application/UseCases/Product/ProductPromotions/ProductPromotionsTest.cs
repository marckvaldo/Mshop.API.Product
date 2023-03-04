using Moq;
using UseCase = MShop.Application.UseCases.Product.Productspromotions;
using UseCaseCommon = MShop.Application.UseCases.Product.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Cache;
using MShop.Business.Interface.Repository;
using Mshop.Test.Business.Entity.Product;
using MShop.Business.Exception;

namespace MShop.UnitTests.Application.UseCases.Product.ProductPromotions
{
    public class ProductPromotionsTest : ProductPromotionsTestFixture
    {
        [Fact(DisplayName = nameof(ProductPromotions))]
        [Trait("Application-UseCase","Product Promotions")]
        public async Task ProductPromotionsCreateCache()
        {
            var cacheRepository = new Mock<ICacheRepository>();
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var productsFake = GetListProdutsOutPut();
            var products = GetListProduts();

            repository.Setup(r => r.GetProductsPromotions()).ReturnsAsync(products);
            cacheRepository.Setup(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()));

            var useCase = new UseCase.ProductsPromotions(cacheRepository.Object,repository.Object,notification.Object);
            var outPut = await useCase.Handle();

            repository.Verify(x => x.GetProductsPromotions(), Times.Once);
            cacheRepository.Verify(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()), Times.Once);
            notification.Verify(x => x.AddNotifications(It.IsAny<string>()), Times.Never);
            cacheRepository.Verify(x => x.SetKeyCollection(It.IsAny<string>(), It.IsAny<object>(),It.IsAny<TimeSpan>()), Times.Once);

            Assert.NotNull(outPut);
            foreach(var item in outPut)
            {
                var product = products.Where(x => x.Id == item.Id).First();
                Assert.NotNull(product);
                Assert.Equal(item.Name, product.Name);
                Assert.Equal(item.Imagem, product.Imagem.Path);
                Assert.Equal(item.Price, product.Price);
                Assert.Equal(item.Description, product.Description);
                Assert.Equal(item.CategoryId, product.CategoryId);
                Assert.Equal(item.Id, product.Id);  
            }
        }


        [Fact(DisplayName = nameof(ProductPromotionsUseCache))]
        [Trait("Application-UseCase", "Product Promotions")]
        public async Task ProductPromotionsUseCache()
        {
            var cacheRepository = new Mock<ICacheRepository>();
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var productsFake = GetListProdutsOutPut();
            var products = GetListProduts();

            cacheRepository.Setup(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>())).ReturnsAsync(productsFake);

            var useCase = new UseCase.ProductsPromotions(cacheRepository.Object, repository.Object, notification.Object);
            var outPut = await useCase.Handle();

            repository.Verify(x => x.GetProductsPromotions(), Times.Never);
            cacheRepository.Verify(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()), Times.Once);
            notification.Verify(x => x.AddNotifications(It.IsAny<string>()), Times.Never);
            cacheRepository.Verify(x => x.SetKeyCollection(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);

            Assert.NotNull(outPut);
            foreach (var item in outPut)
            {
                var product = productsFake.Where(x => x.Id == item.Id).First();
                Assert.NotNull(product);
                Assert.Equal(item.Name, product.Name);
                Assert.Equal(item.Imagem, product.Imagem);
                Assert.Equal(item.Price, product.Price);
                Assert.Equal(item.Description, product.Description);
                Assert.Equal(item.CategoryId, product.CategoryId);
                Assert.Equal(item.Id, product.Id);
            }
        }


        [Fact(DisplayName = nameof(sholdReturnErrorWhenCatGetProductPromotions))]
        [Trait("Application-UseCase", "Product Promotions")]
        public async Task sholdReturnErrorWhenCatGetProductPromotions()
        {
            var cacheRepository = new Mock<ICacheRepository>();
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var productsFake = GetListProdutsOutPut();
            var products = GetListProduts();

            cacheRepository.Setup(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()));
            repository.Setup(x=>x.GetProductsPromotions()).ThrowsAsync(new NotFoundException(""));

            var useCase = new UseCase.ProductsPromotions(cacheRepository.Object, repository.Object, notification.Object);
            var outPut = async () => await useCase.Handle();

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

            repository.Verify(x => x.GetProductsPromotions(), Times.Once);
            cacheRepository.Verify(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()), Times.Once);
            notification.Verify(x => x.AddNotifications(It.IsAny<string>()), Times.Never);
            cacheRepository.Verify(x => x.SetKeyCollection(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);

            

            
        }
    }
}



