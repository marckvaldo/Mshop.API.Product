using Moq;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;
using BusinessEntity = MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Entity;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface.Service;

namespace Mshop.Tests.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductTest : UpdateProductTestFixture
    {
        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Application-UseCase", "Update Product")]
        public async void UpdateProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();

            var request = ProductInPut();
            var productRepository = ProductModelOutPut();

            var productFake = Faker();
            repository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(productFake);

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync($"{productFake.Id}-thumb.jpg");

            var useCase = new ApplicationUseCase.UpdateProduct(repository.Object, notification.Object,storageService.Object);
            var outPut = await useCase.Handler(request);


            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>()),Times.Once);
            notification.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Description, request.Description);
            Assert.Equal(outPut.Price, request.Price);
            Assert.Equal(outPut.CategoryId, request.CategoryId);

        }



        [Fact(DisplayName = nameof(ShoulReturnErroWhenNotFoundUpdateProduct))]
        [Trait("Application-UseCase", "Update Product")]
        public void ShoulReturnErroWhenNotFoundUpdateProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();

            var request = ProductInPut();
            var productRepository = ProductModelOutPut();

            repository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException(""));

            var useCase = new ApplicationUseCase.UpdateProduct(repository.Object, notification.Object, storageService.Object);
            var outPut = async () => await useCase.Handler(request);

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut); 

            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>()), Times.Never);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);

        }



        [Theory(DisplayName = nameof(ShoulReturnErroWhenRequestUpdateProduct))]
        [Trait("Application-UseCase", "Update Product")]
        [MemberData(nameof(GetUpdateProductInPutInvalid))]
        public void ShoulReturnErroWhenRequestUpdateProduct(UpdateProductInPut request)
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();

            //var request = ProductInPut();
            var productRepository = ProductModelOutPut();

            var useCase = new ApplicationUseCase.UpdateProduct(repository.Object, notification.Object, storageService.Object);
            var outPut = async () => await useCase.Handler(request);

            var exception = Assert.ThrowsAsync<ApplicationValidationException>(outPut);

            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>()), Times.Never);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);

        }

    }
}
