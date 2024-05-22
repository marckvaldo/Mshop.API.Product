using Moq;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Business.Interface.Service;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Tests.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductTest : UpdateProductTestFixture
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<INotification> _notifications;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IStorageService> _storageService;
        private readonly Mock<ICategoryRepository> _repositoryCategory;

        public UpdateProductTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _notifications = new Mock<INotification>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _storageService = new Mock<IStorageService>();
            _repositoryCategory = new Mock<ICategoryRepository>();
        }

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Application-UseCase", "Update Product")]
        public async void UpdateProduct()
        {
            var request = ProductInPut();
            var productRepository = ProductModelOutPut();

            var productFake = Faker();
            _productRepository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(productFake);

            var categoryFake = FakerCategory();
            _repositoryCategory.Setup(c => c.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(categoryFake);

            _storageService.Setup(s => s.Upload(It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync($"{productFake.Id}-thumb.jpg");

            var useCase = new ApplicationUseCase.UpdateProduct(
                _productRepository.Object,
                _repositoryCategory.Object,
                _notifications.Object,
                _storageService.Object,
                _unitOfWork.Object);

            var outPut = await useCase.Handle(request, CancellationToken.None);


            _productRepository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>(), CancellationToken.None),Times.Once);
            _notifications.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);
            _unitOfWork.Verify(n=>n.CommitAsync(It.IsAny<CancellationToken>()),Times.Once); 

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Description, request.Description);
            Assert.Equal(outPut.Price, request.Price);
            Assert.Equal(outPut.CategoryId, request.CategoryId);

        }



        [Fact(DisplayName = nameof(ShoulReturnErroWhenNotFoundUpdateProduct))]
        [Trait("Application-UseCase", "Update Product")]
        public void ShoulReturnErroWhenNotFoundUpdateProduct()
        {
            var request = ProductInPut();
            var productRepository = ProductModelOutPut();

            _productRepository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException(""));

            var useCase = new ApplicationUseCase.UpdateProduct(
                _productRepository.Object,
                _repositoryCategory.Object,
                _notifications.Object,
                _storageService.Object,
                _unitOfWork.Object);

            var outPut = async () => await useCase.Handle(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

            _productRepository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>(), CancellationToken.None), Times.Never);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            _unitOfWork.Verify(n => n.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }



        [Theory(DisplayName = nameof(ShoulReturnErroWhenRequestUpdateProduct))]
        [Trait("Application-UseCase", "Update Product")]
        [MemberData(nameof(GetUpdateProductInPutInvalid))]
        public void ShoulReturnErroWhenRequestUpdateProduct(UpdateProductInPut request)
        {
            var categoryFake = FakerCategory();
            _repositoryCategory.Setup(c => c.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(categoryFake);

            var productRepository = ProductModelOutPut();

            var useCase = new ApplicationUseCase.UpdateProduct(
                _productRepository.Object,
                _repositoryCategory.Object,
                _notifications.Object,
                _storageService.Object,
                _unitOfWork.Object);

            var outPut = async () => await useCase.Handle(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

            _productRepository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>(), CancellationToken.None), Times.Never);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            _unitOfWork.Verify(n => n.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

    }
}
