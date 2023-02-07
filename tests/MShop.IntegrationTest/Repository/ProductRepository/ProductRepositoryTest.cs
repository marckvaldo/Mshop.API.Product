using Microsoft.EntityFrameworkCore;
using Moq;
using MShop.Business.Entity;
using MShop.Business.Enum.Paginated;
using MShop.Business.Interface;
using MShop.Business.Paginated;
using MShop.Business.Validation;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraRepository = MShop.Repository.Repository;

namespace MShop.IntegrationTests.Repository.ProductRepository
{
    public class ProductRepositoryTest: ProductRespositoryTesteFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly InfraRepository.ProductRepository _repository;
        public ProductRepositoryTest()
        {
            _DbContext = CreateDBContext();
            _repository = new InfraRepository.ProductRepository(_DbContext);
        }

        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task CreateProduct()
        {
            var product = Faker();

            await _repository.Create(product);

            var newProduct = await _DbContext.Products.FindAsync(product.Id);

            Assert.NotNull(newProduct);
            Assert.Equal(product.Id, newProduct.Id);
            Assert.Equal(product.Name, newProduct.Name);
            Assert.Equal(product.Imagem, newProduct.Imagem);
            Assert.Equal(product.Price, newProduct.Price);
            Assert.Equal(product.Stock, newProduct.Stock);
            Assert.Equal(product.CategoryId, newProduct.CategoryId);
        }

        [Fact(DisplayName = nameof(GetByIdProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task GetByIdProduct()
        {

            var product = Faker();
            var productList = FakerList(20);
            productList.Add(product);
            await _DbContext.AddRangeAsync(productList);
            await _DbContext.SaveChangesAsync();


            var outPut = await _repository.GetById(product.Id);

            Assert.NotNull(outPut);
            Assert.Equal(product.Id, outPut.Id);
            Assert.Equal(product.Name, outPut.Name);
            Assert.Equal(product.Imagem, outPut.Imagem);
            Assert.Equal(product.Price, outPut.Price);
            Assert.Equal(product.Stock, outPut.Stock);
            Assert.Equal(product.CategoryId, outPut.CategoryId);
        }

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task UpdateProduct()
        {
            var notification = new Notifications() ;

            var repository = new InfraRepository.ProductRepository(_DbContext);
            var request = Faker();
            var productList = FakerList(20);
            
            await _DbContext.AddRangeAsync(productList);
            await _DbContext.SaveChangesAsync();

            Guid id = productList.First().Id;

            var product = await _repository.GetById(id);
            
            product.Update(request.Description, request.Name, request.Price, request.CategoryId);
            product.UpdateImage(request.Imagem);
            product.UpdateQuantityStock(request.Stock);
            product.IsValid(notification);

            await repository.Update(product);

            var productUpdate = await (CreateDBContext(true)).Products.FindAsync(id);   

            Assert.NotNull(productUpdate);
            Assert.Equal(id, productUpdate.Id);
            Assert.Equal(request.Name, productUpdate.Name);
            Assert.Equal(request.Imagem, productUpdate.Imagem);
            Assert.Equal(request.Price, productUpdate.Price);
            Assert.Equal(request.Stock, productUpdate.Stock);
            Assert.Equal(request.CategoryId, productUpdate.CategoryId);
        }


        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task DeleteProduct()
        {
            var notification = new Notifications();
            var productList = FakerList(20);

            await _DbContext.AddRangeAsync(productList);
            await _DbContext.SaveChangesAsync();

            var request = productList.First();

            await _repository.DeleteById(request);
            var productUpdate = await (CreateDBContext(true)).Products.FindAsync(request.Id);

            Assert.Null(productUpdate);
        }


        [Fact(DisplayName = nameof(SerachRestusListAndTotal))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task SerachRestusListAndTotal()
        {
             
            var productList = FakerList(20);
            await _DbContext.AddRangeAsync(productList);
            await _DbContext.SaveChangesAsync();

            var perPage = 10;

            var input = new PaginatedInPut(1, perPage, "","",SearchOrder.Asc);

            var outPut = await _repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Count == perPage);
            Assert.Equal(input.PerPage, outPut.PerPage);

            foreach(Product item in outPut.Itens)
            {
                var product = productList.Find(x => x.Id == item.Id);
                Assert.NotNull(product);
                Assert.Equal(product.Name, item.Name);
                Assert.Equal(product.Description, item.Description);
                Assert.Equal(product.Price, item.Price);
            }

        }


        [Fact(DisplayName = nameof(SholdSearchResultListEmpty))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task SholdSearchResultListEmpty()
        {

            var perPage = 20;

            var input = new PaginatedInPut(1, perPage, "", "", SearchOrder.Asc);

            var outPut = await _repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.True(outPut.Itens.Count() == 0);
            Assert.True(outPut.Total == 0);
            Assert.Equal(input.PerPage, outPut.PerPage);
        }

        [Theory(DisplayName = nameof(SerachRestusPaginated))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]
        [InlineData(10,1,10,10)]
        [InlineData(17, 2, 10,7)]
        [InlineData(17, 3, 10, 0)]

        public async Task SerachRestusPaginated(int quantityProduct, int page, int perPage, int expectedQuantityItems)
        {

            var productList = FakerList(quantityProduct);
            await _DbContext.AddRangeAsync(productList);
            await _DbContext.SaveChangesAsync();

            var input = new PaginatedInPut(page, perPage, "", "", SearchOrder.Asc);

            var outPut = await _repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Count() == expectedQuantityItems);
            Assert.Equal(outPut.PerPage, perPage);   
            Assert.True(outPut.Total == quantityProduct);
            Assert.Equal(input.PerPage, outPut.PerPage);

            foreach (Product item in outPut.Itens)
            {
                var product = productList.Find(x => x.Id == item.Id);
                Assert.NotNull(product);
                Assert.Equal(product.Name, item.Name);
                Assert.Equal(product.Description, item.Description);
                Assert.Equal(product.Price, item.Price);
            }

        }


        public void Dispose()
        {
            CleanInMemoryDatabase(_DbContext);
        }
    }
}
