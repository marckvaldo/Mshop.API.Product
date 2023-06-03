using Moq;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.UnitTests.Application.UseCases.Image.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.images.CreateImage;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.UnitTests.Application.UseCases.Image.CreateImage
{
    public class CreateImageTest : CreateImageTestFixture
    {
        [Fact(DisplayName = nameof(CreateImage))]
        [Trait("Application-UseCase", "Create Image")]

        public async void CreateImage()
        {
            var repository = new Mock<IImageRepository>();
            var repositoryProduct = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();
            var unitOfWork = new Mock<IUnitOfWork>();

            repositoryProduct.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(FakerProduct(FakerCategory()));

            var id = Guid.NewGuid();
            var images = ImageFakers64(3);
            var request = FakerRequest(id, images);

            var useCase = new ApplicationUseCase.CreateImage(
                repository.Object,
                storageService.Object, 
                repositoryProduct.Object, 
                notification.Object,
                unitOfWork.Object);

            var outPut = await useCase.Handler(request, CancellationToken.None);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.ProductId, id);
            Assert.NotNull(outPut.Images);
            
        }


        [Fact(DisplayName = nameof(ShouldReturnErrorCreateImageWhenNotHaveImage))]
        [Trait("Application-UseCase", "Create Image")]

        public void ShouldReturnErrorCreateImageWhenNotHaveImage()
        {
            var repository = new Mock<IImageRepository>();
            var notification = new Mock<INotification>();
            var storageService = new Mock<IStorageService>();
            var repositoryProduct = new Mock<IProductRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var id = Guid.NewGuid();
            var images = ImageFakers64(3);
            var request = FakerRequest(id, images);

            request.Images = null;

            var useCase = new ApplicationUseCase.CreateImage(
                repository.Object, 
                storageService.Object, 
                repositoryProduct.Object, 
                notification.Object, 
                unitOfWork.Object);

            var action = async () => await useCase.Handler(request, CancellationToken.None);

            var exception = Assert.ThrowsAnyAsync<ApplicationException>(action);

            repository.Verify(r => r.CreateRange(It.IsAny<List<BusinessEntity.Image>>(), CancellationToken.None), Times.Never);
            notification.Verify(n=>n.AddNotifications(It.IsAny<string>()), Times.Once);
        }

    }
}
