using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MShop.UnitTests.Application.UseCases.Image.Common;
using ApplicationUseCase = MShop.Application.UseCases.Images.GetImage;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Interface.Service;
using MShop.Application.UseCases.Images.GetImage;
using MShop.Business.Exceptions;
using MShop.Application.UseCases.Images.GetImage;

namespace MShop.UnitTests.Application.UseCases.Image.GetImage
{
    public class GetImageTest : ImageBaseFixtureTest
    {
        [Fact(DisplayName = nameof(GetImage))]
        [Trait("Application-UseCase", "Get Image")]
        public async void GetImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();

            var request = Faker(Guid.NewGuid());
            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(request);

            var useCase = new ApplicationUseCase.GetImage(notification.Object, repository.Object, storageService.Object);
            var outPut = await useCase.Handle(new GetImageInPut(Guid.NewGuid()), CancellationToken.None);

            Assert.NotNull(outPut);
            repository.Verify(r=>r.GetById(It.IsAny<Guid>()),Times.Once);
        }



        [Fact(DisplayName = nameof(ShouldReturnErroWhenGetImage))]
        [Trait("Application-UseCase", "Get Image")]
        public void ShouldReturnErroWhenGetImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();

            var request = Faker(Guid.NewGuid());

            var useCase = new ApplicationUseCase.GetImage(notification.Object, repository.Object, storageService.Object);
            var action = async () => await useCase.Handle( new GetImageInPut(Guid.NewGuid()), CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationValidationException>(action);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);            
        }

    }
}
