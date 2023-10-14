using Moq;
using Mshop.Tests.Application.UseCases.Product.GetProduct;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Exception;
using System.Linq.Expressions;
using MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Application.UseCases.Category.GetCategory;

namespace Mshop.Tests.Application.UseCases.Product.GetProduts
{
    public class GetProdutctTest : GetProductTestFixture
    {
        [Fact(DisplayName = nameof(GetProduct))]
        [Trait("Application-UseCase", "Get Products")]
        public async void GetProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var repositoryImage = new Mock<IImageRepository>();

            var productFake = Faker();
            var guid = productFake.Id;
            var imagesFaker = ImageFake(guid);

            repository.Setup(r => r.GetProductWithCategory(It.IsAny<Guid>())).ReturnsAsync(productFake);
            repositoryImage.Setup(r => r.Filter(It.IsAny<Expression<Func<Image, bool>>>())).ReturnsAsync(imagesFaker);

            var useCase = new ApplicationUseCase.GetProduct(repository.Object, repositoryImage.Object, notification.Object) ;
            var outPut = await useCase.Handle(new ApplicationUseCase.GetProductInPut(guid), CancellationToken.None);

            repository.Verify(r => r.GetProductWithCategory(It.IsAny<Guid>()), Times.Once);
            notification.Verify(r => r.AddNotifications(It.IsAny<string>()), Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, productFake.Name);
            Assert.Equal(outPut.Description, productFake.Description);
            Assert.Equal(outPut.Price, productFake.Price);
            Assert.Equal(outPut.Thumb, productFake.Thumb?.Path);
            Assert.Equal(outPut.CategoryId, productFake.CategoryId);
            Assert.Equal(outPut.Stock, productFake.Stock);
            Assert.Equal(outPut.IsActive, productFake.IsActive);
            Assert.NotNull(outPut.Images);
            
            foreach(var item in outPut.Images)
            {
                var image = imagesFaker.Where(i => i.FileName == item).FirstOrDefault();
                Assert.NotNull(image);
                Assert.Equal(image?.FileName,item);
            }

        }


        [Fact(DisplayName = nameof(GetProductWithOutImages))]
        [Trait("Application-UseCase", "Get Products")]
        public async void GetProductWithOutImages()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var repositoryImage = new Mock<IImageRepository>();

            var productFake = Faker();
            var guid = productFake.Id;
            var imagesFaker = ImageFake(guid);

            repository.Setup(r => r.GetProductWithCategory(It.IsAny<Guid>())).ReturnsAsync(productFake);
            repositoryImage.Setup(r => r.Filter(It.IsAny<Expression<Func<Image, bool>>>())).ReturnsAsync(new List<Image>());

            var useCase = new ApplicationUseCase.GetProduct(repository.Object, repositoryImage.Object, notification.Object);
            var outPut = await useCase.Handle(new ApplicationUseCase.GetProductInPut(guid), CancellationToken.None);

            repository.Verify(r => r.GetProductWithCategory(It.IsAny<Guid>()), Times.Once);
            notification.Verify(r => r.AddNotifications(It.IsAny<string>()), Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, productFake.Name);
            Assert.Equal(outPut.Description, productFake.Description);
            Assert.Equal(outPut.Price, productFake.Price);
            Assert.Equal(outPut.Thumb, productFake.Thumb?.Path);
            Assert.Equal(outPut.CategoryId, productFake.CategoryId);
            Assert.Equal(outPut.Stock, productFake.Stock);
            Assert.Equal(outPut.IsActive, productFake.IsActive);
            Assert.True(outPut.Images.Count == 0);
        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProduct))]
        [Trait("Application-UseCase", "Get Products")]
        public void SholdReturnErrorWhenCantGetProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var repositoryImage = new Mock<IImageRepository>();

            repository.Setup(r => r.GetProductWithCategory(It.IsAny<Guid>()));//.ThrowsAsync(new NotFoundException(""));

            var caseUse = new ApplicationUseCase.GetProduct(repository.Object, repositoryImage.Object, notification.Object);
            var outPut = async () => await caseUse.Handle(new ApplicationUseCase.GetProductInPut(Guid.NewGuid()), CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationValidationException>(outPut);

            repository.Verify(r => r.GetProductWithCategory(It.IsAny<Guid>()), Times.Once);
            notification.Verify(r => r.AddNotifications(It.IsAny<string>()), Times.Once);
        }
    }
}
