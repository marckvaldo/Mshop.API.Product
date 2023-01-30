using Moq;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.CreateProducts;
using BusinessRepository = MShop.Business.Interface.Repository;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Exception;


namespace Mshop.Tests.Application.UseCases.Product.CreateProduct
{
    public class CreateProductTest: CreateProductTestFixture
    {
        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Application-UseCase", "Create Products")]
        public async void CreateProduct()
        {
            var repository = new Mock<BusinessRepository.IProductRepository>();
            var notification = new Mock<INotification>();


            var productUseCase = new ApplicationUseCase.CreateProduct(repository.Object,notification.Object);
            
            var request = Faker();

            var outPut =  await productUseCase.Handle(request);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>()),
                Times.Once);

            notification.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Description, request.Description);
            Assert.Equal(outPut.Price, request.Price);
            Assert.Equal(outPut.Imagem, request.Imagem);
            Assert.Equal(outPut.CategoryId, request.CategoryId);
            Assert.Equal(outPut.Stock, request.Stock);
            Assert.Equal(outPut.IsActive, request.IsActive);
            
        }

        [Theory(DisplayName = nameof(SholdReturnErrorWhenCantCreateProduct))]
        [Trait("Application-UseCase", "Create Products")]
        [MemberData(nameof(GetCreateProductInPutInvalid))]
        public async void SholdReturnErrorWhenCantCreateProduct(CreateProductInPut request)
        {

            var repository = new Mock<BusinessRepository.IProductRepository>();
            var notification = new Mock<INotification>();

            var productUseCase = new ApplicationUseCase.CreateProduct(repository.Object, notification.Object);

            var outPut = await productUseCase.Handle(request);

            repository.Verify(
                repository => repository.Create(It.IsAny<BusinessEntity.Product>()),
                Times.Once);

            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Description, request.Description);
            Assert.Equal(outPut.Price, request.Price);
            Assert.Equal(outPut.Imagem, request.Imagem);
            Assert.Equal(outPut.CategoryId, request.CategoryId);
            Assert.Equal(outPut.Stock, request.Stock);
            Assert.Equal(outPut.IsActive, request.IsActive);

        }
    }
}
