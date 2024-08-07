﻿using Moq;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Tests.Application.UseCases.Product.UpdateStockProduct
{
    public class UpdateStockProductTest :  UpdateStockProductTestFixture
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<INotification> _notifications;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public UpdateStockProductTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _notifications = new Mock<INotification>();
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact(DisplayName = nameof(UpdateStockProduct))]
        [Trait("Application-UseCase", "Update Stock Product")]
        public async void UpdateStockProduct()
        {
            var request = UpdateStockProductInPut();

            _productRepository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Faker());
            
            var useCase = new ApplicationUseCase.UpdateStockProducts(
                _productRepository.Object,
                _notifications.Object,
                _unitOfWork.Object);

            var outPut = await useCase.Handle(request, CancellationToken.None);

            _productRepository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>(), CancellationToken.None), Times.Once);
            _notifications.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);
            _unitOfWork.Verify(n=>n.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(request.Stock,result.Stock);
            
        }


        [Fact(DisplayName = nameof(SholdReturnErrorCantUpdateStockProduct))]
        [Trait("Application-UseCase", "Update Stock Product")]
        public void SholdReturnErrorCantUpdateStockProduct()
        {
            var request = UpdateStockProductInPut();
            request.Stock = request.Stock * -1;  

            _notifications.Setup(n=>n.HasErrors()).Returns(true);

            var useCase = new ApplicationUseCase.UpdateStockProducts(
                _productRepository.Object,
                _notifications.Object,
                _unitOfWork.Object);

            var outPut = async () => await useCase.Handle(request, CancellationToken.None);

            var excption = Assert.ThrowsAsync<NotFoundException>(outPut);

            _productRepository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>(), CancellationToken.None), Times.Never);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);
            _unitOfWork.Verify(n => n.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

    }
}
