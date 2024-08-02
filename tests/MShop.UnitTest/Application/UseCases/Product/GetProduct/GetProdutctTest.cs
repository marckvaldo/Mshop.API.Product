using Moq;
using Mshop.Tests.Application.UseCases.Product.GetProduct;
using MShop.Business.Entity;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using System.Linq.Expressions;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;

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

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(result.Name, productFake.Name);
            Assert.Equal(result.Description, productFake.Description);
            Assert.Equal(result.Price, productFake.Price);
            Assert.Equal(result.Thumb, productFake.Thumb?.Path);
            Assert.Equal(result.CategoryId, productFake.CategoryId);
            Assert.Equal(result.Stock, productFake.Stock);
            Assert.Equal(result.IsActive, productFake.IsActive);
            Assert.NotNull(result.Images);
            
            foreach(var item in result.Images)
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

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(result.Name, productFake.Name);
            Assert.Equal(result.Description, productFake.Description);
            Assert.Equal(result.Price, productFake.Price);
            Assert.Equal(result.Thumb, productFake.Thumb?.Path);
            Assert.Equal(result.CategoryId, productFake.CategoryId);
            Assert.Equal(result.Stock, productFake.Stock);
            Assert.Equal(result.IsActive, productFake.IsActive);
            Assert.True(result.Images.Count == 0);
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
