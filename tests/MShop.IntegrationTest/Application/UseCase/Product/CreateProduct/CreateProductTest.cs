
using Microsoft.EntityFrameworkCore;
using Moq;
using MShop.Business.Interface;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Product.CreateProducts;

namespace MShop.IntegrationTests.Application.UseCase.Product.CreateProduct
{
    public class CreateProductTest : CreateProductTestFixture, IDisposable
    {
        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]
        public async void CreateProduct()
        {

            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new ProductRepository(dbContext);
            var notification = new Notifications();

            var request = Faker();
            var productUseCase = new ApplicationUseCase.CreateProduct(repository, notification);
            var outPut = await productUseCase.Handle(request);

            var newProduct = await CreateDBContext(true).Products.FindAsync(outPut.Id);
            
            Assert.False(notification.HasErrors());
            Assert.NotNull(outPut);
            Assert.NotNull(newProduct);
            Assert.Equal(outPut.Name, newProduct.Name);
            Assert.Equal(outPut.Description, newProduct.Description);
            Assert.Equal(outPut.Price, newProduct.Price);
            Assert.Equal(outPut.Imagem, newProduct.Imagem);
            Assert.Equal(outPut.CategoryId, newProduct.CategoryId);
            Assert.Equal(outPut.Stock, newProduct.Stock);
            Assert.Equal(outPut.IsActive, newProduct.IsActive);


            Assert.Equal(request.Name, outPut.Name);
            Assert.Equal(request.Description, outPut.Description);
            Assert.Equal(request.Price, outPut.Price);
            Assert.Equal(request.Imagem, outPut.Imagem);
            Assert.Equal(request.CategoryId, outPut.CategoryId);
            Assert.Equal(request.Stock, outPut.Stock);
            Assert.Equal(request.IsActive, outPut.IsActive);

        }



        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
