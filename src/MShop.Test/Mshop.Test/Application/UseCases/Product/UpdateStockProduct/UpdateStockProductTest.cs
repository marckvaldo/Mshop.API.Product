using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;
using BusinessEntity = MShop.Business.Entity;
using MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Business.Exception;

namespace Mshop.Tests.Application.UseCases.Product.UpdateStockProduct
{
    public class UpdateStockProductTest :  UpdateStockProductTestFixture
    {
        [Fact(DisplayName = nameof(UpdateStockProduct))]
        [Trait("Application-UseCase", "Update Stock Product")]
        public async void UpdateStockProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            

            var request = UpdateStockProductInPut();

            repository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Faker());
            
            var useCase = new ApplicationUseCase.UpdateStockProducts(repository.Object, notification.Object);
            var outPut = await useCase.Handle(request);

            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>()), Times.Once);
            notification.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(request.Stock,outPut.Stock);
            
        }

        [Fact(DisplayName = nameof(SholdReturnErrorCantUpdateStockProduct))]
        [Trait("Application-UseCase", "Update Stock Product")]
        public async void SholdReturnErrorCantUpdateStockProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();


            var request = UpdateStockProductInPut();

            repository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Faker());

            var useCase = new ApplicationUseCase.UpdateStockProducts(repository.Object, notification.Object);
            var outPut = async () => await useCase.Handle(request);

            var excption = Assert.ThrowsAsync<NotFoundException>(outPut);

            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);

        }

    }
}
