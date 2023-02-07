using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MShop.Business.Interface.Repository;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    public class UpdateProductTest : UpdateProdutTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;

        public UpdateProductTest()
        {
            _DbContext = CreateDBContext(false, "UpdateProductTest");
            _repository = new ProductRepository(_DbContext);
        }

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task UpdateProduct()
        {
           
            var notificacao = new Notifications();

            var product = Faker();
            var request = RequestFake();
            

            await _DbContext.AddAsync(product);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.UpdateProduct(_repository, notificacao);
            var outPut = await useCase.Handle(request);

            var productDb = await _DbContext.Products.Where(x=>x.Id == product.Id).FirstAsync();

            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(outPut.Name, productDb.Name);  
            Assert.Equal(outPut.Description, productDb.Description);  
            Assert.Equal(outPut.Imagem, productDb.Imagem);
            Assert.Equal(outPut.Price, productDb.Price);  
            Assert.Equal(outPut.CategoryId, productDb.CategoryId);
            Assert.NotEmpty(outPut.Name);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase(_DbContext);
        }
    }
}
