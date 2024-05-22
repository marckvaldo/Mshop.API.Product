using Moq;
using MShop.Core.Cache;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using UseCase = MShop.Application.UseCases.Product.Productspromotions;
using UseCaseCommon = MShop.Application.UseCases.Product.Common;

namespace MShop.UnitTests.Application.UseCases.Product.ProductPromotions
{
    public class ProductPromotionsTest : ProductPromotionsTestFixture
    {
        private readonly Mock<INotification> _notifications;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IProductRepository> _repositoryProduct;
        private readonly Mock<ICacheRepository> _cacheRepository;

        public ProductPromotionsTest()
        {
            _notifications = new Mock<INotification>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _repositoryProduct = new Mock<IProductRepository>();
            _cacheRepository = new Mock<ICacheRepository>();
        }


        [Fact(DisplayName = nameof(ProductPromotions))]
        [Trait("Application-UseCase","Product Promotions")]
        public async Task ProductPromotionsCreateCache()
        {
            var productsFake = GetListProdutsOutPut();
            var products = GetListProduts();

            _repositoryProduct.Setup(r => r.GetProductsPromotions()).ReturnsAsync(products);
            _cacheRepository.Setup(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()));

            var useCase = new UseCase.ProductsPromotions(_cacheRepository.Object, _repositoryProduct.Object, _notifications.Object);
            var outPut = await useCase.Handler();

            _repositoryProduct.Verify(x => x.GetProductsPromotions(), Times.Once);
            _cacheRepository.Verify(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()), Times.Once);
            _notifications.Verify(x => x.AddNotifications(It.IsAny<string>()), Times.Never);
            _cacheRepository.Verify(x => x.SetKeyCollection(It.IsAny<string>(), It.IsAny<object>(),It.IsAny<TimeSpan>()), Times.Once);

            Assert.NotNull(outPut);
            foreach(var item in outPut)
            {
                var product = products.Where(x => x.Id == item.Id).First();
                Assert.NotNull(product);
                Assert.Equal(item.Name, product.Name);
                Assert.Equal(item.Thumb, product.Thumb.Path);
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
            var productsFake = GetListProdutsOutPut();
            var products = GetListProduts();

            _cacheRepository.Setup(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>())).ReturnsAsync(productsFake);

            var useCase = new UseCase.ProductsPromotions(_cacheRepository.Object, _repositoryProduct.Object, _notifications.Object);
            var outPut = await useCase.Handler();

            _repositoryProduct.Verify(x => x.GetProductsPromotions(), Times.Never);
            _cacheRepository.Verify(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()), Times.Once);
            _notifications.Verify(x => x.AddNotifications(It.IsAny<string>()), Times.Never);
            _cacheRepository.Verify(x => x.SetKeyCollection(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);

            Assert.NotNull(outPut);
            foreach (var item in outPut)
            {
                var product = productsFake.Where(x => x.Id == item.Id).First();
                Assert.NotNull(product);
                Assert.Equal(item.Name, product.Name);
                Assert.Equal(item.Thumb, product.Thumb);
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
            var productsFake = GetListProdutsOutPut();
            var products = GetListProduts();

            _cacheRepository.Setup(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()));
            _repositoryProduct.Setup(x=>x.GetProductsPromotions()).ThrowsAsync(new NotFoundException(""));

            var useCase = new UseCase.ProductsPromotions(_cacheRepository.Object, _repositoryProduct.Object, _notifications.Object);
            var outPut = async () => await useCase.Handler();

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

            _repositoryProduct.Verify(x => x.GetProductsPromotions(), Times.Once);
            _cacheRepository.Verify(x => x.GetKeyCollection<UseCaseCommon.ProductModelOutPut>(It.IsAny<string>()), Times.Once);
            _notifications.Verify(x => x.AddNotifications(It.IsAny<string>()), Times.Never);
            _cacheRepository.Verify(x => x.SetKeyCollection(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        }
    }
}



