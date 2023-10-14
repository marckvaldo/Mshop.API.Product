using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Images.DeleteImage;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Entity;
using MShop.Application.UseCases.Images.DeleteImage;

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
            var unitOfWork = new Mock<IUnitOfWork>();


            var id = Guid.NewGuid();
            var images = Faker(id);
            var request = FakerRequest();

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(images);
            storageService.Setup(s => s.Delete(It.IsAny<string>())).ReturnsAsync(true);

            var useCase = new ApplicationUseCase.DeleteImage(
                repository.Object, 
                storageService.Object, 
                notification.Object,
                unitOfWork.Object);

            var outPut = await useCase.Handle(new DeleteImageInPut(request), CancellationToken.None);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.ProductId, id);
            Assert.NotNull(outPut.Image);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }



        [Fact(DisplayName = nameof(ShouldReturnErrorWhenDeleteImage))]
        [Trait("Application-UseCase", "Delete Image")]

        public void ShouldReturnErrorWhenDeleteImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var id = Guid.NewGuid();
            var images = Faker(id);
            var request = FakerRequest();
         

            var useCase = new ApplicationUseCase.DeleteImage(
                repository.Object, 
                storageService.Object, 
                notification.Object,
                unitOfWork.Object);

            var action = async () => await useCase.Handle(new DeleteImageInPut(request), CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationException>(action);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);
            storageService.Verify(s => s.Delete(It.IsAny<string>()), Times.Never);
            repository.Verify(r => r.DeleteById(It.IsAny<BusinessEntity.Image>(),CancellationToken.None), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);


        }
    }
}
