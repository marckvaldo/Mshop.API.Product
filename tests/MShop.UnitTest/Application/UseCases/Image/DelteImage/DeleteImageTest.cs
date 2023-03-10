using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.images.DeleteImage;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Entity;

namespace MShop.UnitTests.Application.UseCases.Image.DelteImage
{
    public class DeleteImageTest : DeleteImageTestFixute
    {
        [Fact(DisplayName = nameof(DeleteImage))]
        [Trait("Application-UseCase", "Delete Image")]

        public async void DeleteImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();


            var id = Guid.NewGuid();
            var images = Faker(id);
            var request = FakerRequest();

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(images);

            var useCase = new ApplicationUseCase.DeleteImage(repository.Object, storageService.Object, notification.Object);
            var outPut = await useCase.Handler(request);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.ProductId, id);
            Assert.NotNull(outPut.Image);

        }



        [Fact(DisplayName = nameof(ShouldReturnErrorWhenDeleteImage))]
        [Trait("Application-UseCase", "Delete Image")]

        public void ShouldReturnErrorWhenDeleteImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();

            var id = Guid.NewGuid();
            var images = Faker(id);
            var request = FakerRequest();
         

            var useCase = new ApplicationUseCase.DeleteImage(repository.Object, storageService.Object, notification.Object);
            var action = async () => await useCase.Handler(request);

            var exception = Assert.ThrowsAsync<ApplicationException>(action);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);
            storageService.Verify(s => s.Delete(It.IsAny<string>()), Times.Never);
            repository.Verify(r => r.DeleteById(It.IsAny<BusinessEntity.Image>()), Times.Never);


        }
    }
}
