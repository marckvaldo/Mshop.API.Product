using Moq;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.CreateProducts;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Interface.Service;
using MShop.Business.Interface.Repository;
using MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Business.Validation;

namespace Mshop.Tests.Application.UseCases.Product.CreateProduct
{
    public class CreateProductTest: CreateProductTestFixture
    {
        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Application-UseCase", "Create Products")]
        public async void CreateProduct()
        {
            var repository = new Mock<IProductRepository>();
            //var notification = new Mock<INotification>();
            var notification = new Notifications();
            var storageService = new Mock<IStorageService>();
            var repositoryCategoria = new Mock<ICategoryRepository>();
            var repositoryImage = new Mock<IImageRepository>();

            var request = Faker();
            var categoryFake = new Category(faker.Commerce.Categories(1)[0], true);
            var nameImage = $"{request.Name}-thumb.{request.Thumb?.Extension}";

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(nameImage);
            repositoryCategoria.Setup(c => c.GetById(It.IsAny<Guid>())).ReturnsAsync(categoryFake);

            var productUseCase = new ApplicationUseCase.CreateProduct(repository.Object, 
                notification, 
                repositoryCategoria.Object, 
                storageService.Object, 
                repositoryImage.Object);
            
           

            var outPut =  await productUseCase.Handler(request);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>()),
                Times.Once);

            //notification.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Description, request.Description);
            Assert.Equal(outPut.Price, request.Price);
            Assert.Equal(outPut.Thumb, nameImage);
            Assert.Equal(outPut.CategoryId, request.CategoryId);
            Assert.Equal(outPut.Stock, request.Stock);
            Assert.Equal(outPut.IsActive, request.IsActive);
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

            var categoryFake = new Category(faker.Commerce.Categories(1)[0], true);
            categoryFake.Id = request.CategoryId;
            var nameImage = $"{request.Name}-thumb.jpg";

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(nameImage);
            repositoryCategoria.Setup(c => c.GetById(It.IsAny<Guid>())).ReturnsAsync(categoryFake);

            var productUseCase = new ApplicationUseCase.CreateProduct(
                repository.Object, 
                notification, 
                repositoryCategoria.Object, 
                storageService.Object, 
                repositoryImage.Object);

            var action = async () => await productUseCase.Handler(request);

            var exception =  Assert.ThrowsAsync<EntityValidationException>(action);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>()),
                Times.Never);

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

            var request = Faker();
            var categoryFake = new Category(faker.Commerce.Categories(1)[0], true);
            var nameImage = $"{request.Name}-thumb.{request.Thumb?.Extension}";

            storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(nameImage);            

            var productUseCase = new ApplicationUseCase.CreateProduct(repository.Object,
                notification,
                repositoryCategoria.Object,
                storageService.Object,
                repositoryImage.Object);

            var action = async () => await productUseCase.Handler(request);

            var exception = Assert.ThrowsAsync<ApplicationException>(action);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>()),
                Times.Never);

            Assert.True(notification.HasErrors());


        }
    }
}

