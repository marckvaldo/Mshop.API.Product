using Moq;
using MShop.Application.UseCases.Images.GetImage;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Interface;
using MShop.UnitTests.Application.UseCases.Image.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApplicationUseCase = MShop.Application.UseCases.Images.ListImage;
using BusinessEntity = MShop.Business.Entity;
using MShop.Application.UseCases.Images.ListImage;
using System.Linq.Expressions;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Application.UseCases.Images.ListImage;

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

            repository.Verify(r => r.Filter(It.IsAny<Expression<Func<BusinessEntity.Image, bool>>>()), Times.Once);
            Assert.NotNull(outPut);

            foreach (var item in outPut.Images) 
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
