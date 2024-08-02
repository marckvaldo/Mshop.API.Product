using Moq;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Entity;
using MShop.Business.Interface.Service;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.CreateProducts;
using BusinessEntity = MShop.Business.Entity;
using MShop.Core.Data;

namespace Mshop.Tests.Application.UseCases.Product.CreateProduct
{
    public class CreateProductTest: CreateProductTestFixture
    {
        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Application-UseCase", "Create Products")]
        public async void CreateProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Notifications();
            var storageService = new Mock<IStorageService>();
            var repositoryCategoria = new Mock<ICategoryRepository>();
            var repositoryImage = new Mock<IImageRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var request = Faker();
            var categoryFake = new Category(faker.Commerce.Categories(1)[0], true);
            var nameImage = $"{request.Name}-thumb.{ExtensionFile(request.Thumb.FileStremBase64)}";

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(nameImage);
            repositoryCategoria.Setup(c => c.GetById(It.IsAny<Guid>())).ReturnsAsync(categoryFake);
            unitOfWork.Setup(u=>u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var productUseCase = new ApplicationUseCase.CreateProduct(
                repository.Object, 
                notification, 
                repositoryCategoria.Object, 
                storageService.Object,
                unitOfWork.Object);
            
            var outPut =  await productUseCase.Handle(request, CancellationToken.None);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>(),CancellationToken.None),
                Times.Once);

            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(result.Name, request.Name);
            Assert.Equal(result.Description, request.Description);
            Assert.Equal(result.Price, request.Price);
            Assert.Equal(result.CategoryId, request.CategoryId);
            Assert.Equal(result.Stock, request.Stock);
            Assert.Equal(result.IsActive, request.IsActive);
            Assert.False(notification.HasErrors());

        }

        
        [Theory(DisplayName = nameof(SholdReturnErrorWhenCantCreateProduct))]
        [Trait("Application-UseCase", "Create Products")]
        [MemberData(nameof(GetCreateProductInPutInvalid))]
        public void SholdReturnErrorWhenCantCreateProduct(CreateProductInPut request)
        {

            var repository = new Mock<IProductRepository>();
            //var notification = new Mock<INotification>();
            var notification = new Notifications();
            var storageService = new Mock<IStorageService>();
            var repositoryCategoria = new Mock<ICategoryRepository>();
            var repositoryImage = new Mock<IImageRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var categoryFake = new Category(faker.Commerce.Categories(1)[0], true);
            categoryFake.Id = request.CategoryId;
            var nameImage = $"{request.Name}-thumb.jpg";

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(nameImage);
            repositoryCategoria.Setup(c => c.GetById(It.IsAny<Guid>())).ReturnsAsync(categoryFake);
            unitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()));

            var productUseCase = new ApplicationUseCase.CreateProduct(
                repository.Object, 
                notification, 
                repositoryCategoria.Object, 
                storageService.Object, 
                unitOfWork.Object);

            var action = async () => await productUseCase.Handle(request,CancellationToken.None);

            var exception =  Assert.ThrowsAsync<EntityValidationException>(action);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>(), CancellationToken.None),
                Times.Never);

            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

            Assert.True(notification.HasErrors());
        }


        [Fact(DisplayName = nameof(ShoudReturnErroWhenCreateProductWhenThereIsNotCategory))]
        [Trait("Application-UseCase", "Create Products")]
        public void ShoudReturnErroWhenCreateProductWhenThereIsNotCategory()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Notifications();
            var storageService = new Mock<IStorageService>();
            var repositoryCategoria = new Mock<ICategoryRepository>();
            var repositoryImage = new Mock<IImageRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();   

            var request = Faker();
            var categoryFake = new Category(faker.Commerce.Categories(1)[0], true);
            var nameImage = $"{request.Name}-thumb.{ExtensionFile(request.Thumb.FileStremBase64)}";

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(nameImage);
           

            var productUseCase = new ApplicationUseCase.CreateProduct(
                repository.Object,
                notification,
                repositoryCategoria.Object,
                storageService.Object,
                unitOfWork.Object);

            var action = async () => await productUseCase.Handle(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationException>(action);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>(), CancellationToken.None),
                Times.Never);

            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

            //Assert.True(notification.HasErrors());


        }
    }
}

