using Moq;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;
using BusinessEntity = MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Entity;
using MShop.Application.UseCases.Product.UpdateProduct;

namespace Mshop.Tests.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductTest : UpdateProductTestFixture
    {
        [Fact(DisplayName = "Update Products")]
        [Trait("Application-UseCase", "Update Product")]
        public async void UpdateProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var request = ProductInPut();
            var productRepository = ProductModelOutPut();

            repository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Faker());

            var useCase = new ApplicationUseCase.UpdateProduct(repository.Object, notification.Object);
            var outPut = await useCase.Handle(request);


            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Product>()),Times.Once);
            notification.Verify(n=>n.AddNotifications(It.IsAny<string>()),Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Description, request.Description);
            Assert.Equal(outPut.Imagem, request.Imagem);
            Assert.Equal(outPut.Price, request.Price);
            Assert.Equal(outPut.CategoryId, request.CategoryId);

        }
    }
}
