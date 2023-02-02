using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Repository.Repository;
using MShop.Repository.Context;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;
using MShop.Business.Validation;
using MShop.Business.Exception;

namespace MShop.IntegrationTests.Application.UseCase.Product.GetProduct
{
    public class GetProductTest:GetProductTestFixture
    {
        [Fact(DisplayName = nameof(GetProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]
        public async void GetProduct()
        {
            RepositoryDbContext dbContext = CreateDBContext();  

            var repository = new ProductRepository(dbContext);
            var notification = new Notifications();

            var productFake = Faker();
            dbContext.Products.Add(productFake);
            await dbContext.SaveChangesAsync();

            var guid = productFake.Id;

          
            var useCase = new ApplicationUseCase.GetProduct(repository, notification);
            var outPut = await useCase.Handle(guid);


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
        [Trait("Application-UseCase", "Product Use Case")]
        public async void SholdReturnErrorWhenCantGetProduct()
        {
            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new ProductRepository(dbContext);
            var notification = new Notifications();

            var productFake = Faker();
            dbContext.Products.Add(productFake);
            await dbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.GetProduct(repository, notification);
            var outPut = async () => await useCase.Handle(Guid.NewGuid());

            var exception = await Assert.ThrowsAsync<NotFoundException>(outPut);
            Assert.Equal(exception.Message, "your search returned null");

        }
    }
}
