using Moq;
using Mshop.Tests.Application.UseCases.Product.GetProduct;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Exception;

namespace Mshop.Tests.Application.UseCases.Product.GetProduts
{
    public class GetProdutctTest : GetProductTestFixture
    {
        [Fact(DisplayName = nameof(GetProduct))]
        [Trait("Application-UseCase", "Get Products")]
        public async void GetProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var productFake = Faker();
            var guid = productFake.Id;

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(productFake);

            var useCase = new ApplicationUseCase.GetProduct(repository.Object, notification.Object);
            var outPut = await useCase.Handle(guid);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, productFake.Name);
            Assert.Equal(outPut.Description, productFake.Description);
            Assert.Equal(outPut.Price, productFake.Price);
            Assert.Equal(outPut.Imagem, productFake.Imagem);
            Assert.Equal(outPut.CategoryId, productFake.CategoryId);
            Assert.Equal(outPut.Stock, productFake.Stock);
            Assert.Equal(outPut.IsActive, productFake.IsActive);

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProduct))]
        [Trait("Application-UseCase", "Get Products")]
        public async void SholdReturnErrorWhenCantGetProduct()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException(""));

            var caseUse = new ApplicationUseCase.GetProduct(repository.Object, notification.Object);
            var outPut = async () => await caseUse.Handle(Guid.NewGuid());

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
