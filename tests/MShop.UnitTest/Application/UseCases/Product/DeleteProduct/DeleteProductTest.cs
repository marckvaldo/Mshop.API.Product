﻿using Moq;
using MShop.Business.Interface.Service;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
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
            var repositoryImage = new Mock<IImageRepository>();
            var storageService = new Mock<IStorageService>();
            var unitOfWork = new Mock<IUnitOfWork>();   

            repository.Setup(repository => repository.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Faker());

            var guid = Faker().Id;

            var product = new ApplicationUseCase.DeleteProduct(
                repository.Object, 
                repositoryImage.Object, 
                notification.Object, 
                storageService.Object,
                unitOfWork.Object);

            var outPut = await product.Handle(new ApplicationUseCase.DeleteProductInPut(guid), CancellationToken.None);

            repository.Verify(r => r.DeleteById(It.IsAny<BusinessEntity.Product>(), CancellationToken.None),Times.Once);
            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(outPut.Id, guid);
            Assert.NotNull(outPut);            

        }

        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantDeleteProduct))]
        [Trait("Application-UseCase", "Delete Products")]
        public void SholdReturnErrorWhenCantDeleteProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var repositoryImage = new Mock<IImageRepository>();
            var storageService = new Mock<IStorageService>();
            var unitOfWork = new Mock<IUnitOfWork>();

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException("your search returned null"));

            var product = new ApplicationUseCase.DeleteProduct(
                repository.Object, 
                repositoryImage.Object, 
                notification.Object, 
                storageService.Object,
                unitOfWork.Object);

            var guid = Faker().Id;
            var action = async () => await product.Handle(new ApplicationUseCase.DeleteProductInPut(guid), CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(action);

            repository.Verify(r => r.DeleteById(It.IsAny<BusinessEntity.Product>(), CancellationToken.None), Times.Never);
            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(a => a.AddNotifications(It.IsAny<string>()), Times.Never);
            repositoryImage.Verify(a => a.DeleteByIdProduct(It.IsAny<Guid>()), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
