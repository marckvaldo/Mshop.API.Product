using Moq;
using MShop.Application.UseCases.Images.ListImage;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using MShop.UnitTests.Application.UseCases.Image.Common;
using System.Linq.Expressions;
using ApplicationUseCase = MShop.Application.UseCases.Images.ListImage;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.UnitTests.Application.UseCases.Image.ListImage
{
    public class ListImageTest : ImageBaseFixtureTest
    {
        [Fact(DisplayName = nameof(ListImage))]
        [Trait("Application-UseCase", "List Image")]
        public async void ListImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();

            var productId = Guid.NewGuid();
            var imagens = FakerImages(productId, 3);

            var request = Faker(productId);
            repository.Setup(r => r.Filter(It.IsAny<Expression<Func<BusinessEntity.Image, bool>>>())).ReturnsAsync(imagens);

            var useCase = new ApplicationUseCase.ListImage(notification.Object, repository.Object);
            var outPut = await useCase.Handle( new ListImageInPut(productId), CancellationToken.None);

            var result = outPut.Data;

            repository.Verify(r => r.Filter(It.IsAny<Expression<Func<BusinessEntity.Image, bool>>>()), Times.Once);
            Assert.NotNull(result);

            foreach (var item in result.Images) 
            {
                var image = imagens.Where(x=>x.FileName == item.Image).FirstOrDefault();   

                Assert.NotNull(image);
                Assert.Equal(image?.FileName, item.Image);
            }
        }



        [Fact(DisplayName = nameof(ShoudReturNullWhenListImage))]
        [Trait("Application-UseCase", "List Image")]
        public void ShoudReturNullWhenListImage()
        {
            var notification = new Mock<INotification>();
            var repository = new Mock<IImageRepository>();

            repository.Setup(r => r.Filter(It.IsAny<Expression<Func<BusinessEntity.Image, bool>>>())).ThrowsAsync(new NotFoundException(""));

            var useCase = new ApplicationUseCase.ListImage(notification.Object, repository.Object);
            var outPut = async () => await useCase.Handle(new ListImageInPut(Guid.NewGuid()), CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

        }
    }
}
