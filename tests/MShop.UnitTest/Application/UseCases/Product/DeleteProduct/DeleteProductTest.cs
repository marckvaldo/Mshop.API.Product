using Moq;
using MShop.Business.Exception;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.DeleteProduct;
using BusinessEntity = MShop.Business.Entity;




namespace Mshop.Tests.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProductTest : DeleteProductTestFixture
    {
        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Application-UseCase", "Delete Products")]

        public async void DeleteProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            repository.Setup(repository => repository.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Faker());

            var product = new ApplicationUseCase.DeleteProduct(repository.Object, notification.Object);

            var guid = Faker().Id;
            var outPut = await product.Handle(guid);

            repository.Verify(r => r.DeleteById(It.IsAny<BusinessEntity.Product>()),Times.Once);
            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);

            Assert.Equal(outPut.Id, guid);
            Assert.NotNull(outPut);

        }

        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantDeleteProduct))]
        [Trait("Application-UseCase", "Delete Products")]
        public async void SholdReturnErrorWhenCantDeleteProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException("your search returned null"));

            var product = new ApplicationUseCase.DeleteProduct(repository.Object, notification.Object);
            var guid = Faker().Id;
            var action = async () => await product.Handle(guid);

            var exception = Assert.ThrowsAsync<NotFoundException>(action);

            repository.Verify(r => r.DeleteById(It.IsAny<BusinessEntity.Product>()), Times.Never);
            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(a => a.AddNotifications(It.IsAny<string>()), Times.Never);
        }
    }
}
