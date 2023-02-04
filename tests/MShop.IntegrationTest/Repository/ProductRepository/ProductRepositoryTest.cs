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
        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async void CreateProduct()
        {
            
            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new InfraRepository.ProductRepository(dbContext);

            var product = Faker();

            await repository.Create(product);

            var newProduct = await dbContext.Products.FindAsync(product.Id);

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

        public async void GetByIdProduct()
        {

            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new InfraRepository.ProductRepository(dbContext);
            var product = Faker();
            var productList = FakerList(20);
            productList.Add(product);
            await dbContext.AddRangeAsync(productList);
            await dbContext.SaveChangesAsync();


            var outPut = await repository.GetById(product.Id);

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

        public async void UpdateProduct()
        {

            RepositoryDbContext dbContext = CreateDBContext();
            var notification = new Notifications() ;

            var repository = new InfraRepository.ProductRepository(dbContext);
            var request = Faker();
            var productList = FakerList(20);
            
            await dbContext.AddRangeAsync(productList);
            await dbContext.SaveChangesAsync();

            Guid id = productList.First().Id;

            var product = await repository.GetById(id);
            
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

        public async void DeleteProduct()
        {

            RepositoryDbContext dbContext = CreateDBContext();
            var notification = new Notifications();

            var repository = new InfraRepository.ProductRepository(dbContext);
            var productList = FakerList(20);

            await dbContext.AddRangeAsync(productList);
            await dbContext.SaveChangesAsync();

            var request = productList.First();

            await repository.DeleteById(request);
            var productUpdate = await (CreateDBContext(true)).Products.FindAsync(request.Id);

            Assert.Null(productUpdate);
        }


        [Fact(DisplayName = nameof(SerachRestusListAndTotal))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async void SerachRestusListAndTotal()
        {
             
            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new InfraRepository.ProductRepository(dbContext);
            var productList = FakerList(20);
            await dbContext.AddRangeAsync(productList);
            await dbContext.SaveChangesAsync();

            var perPage = 10;

            var input = new PaginatedInPut(1, perPage, "","",SearchOrder.Asc);

            var outPut = await repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Count == perPage);
            //Assert.True(outPut.Itens.Count == outPut.Total);
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

        public async void SholdSearchResultListEmpty()
        {

            RepositoryDbContext dbContext = CreateDBContext();
            var repository = new InfraRepository.ProductRepository(dbContext);
            var perPage = 20;

            var input = new PaginatedInPut(1, perPage, "", "", SearchOrder.Asc);

            var outPut = await repository.FilterPaginated(input);

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

        public async void SerachRestusPaginated(int quantityProduct, int page, int perPage, int expectedQuantityItems)
        {

            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new InfraRepository.ProductRepository(dbContext);
            var productList = FakerList(quantityProduct);
            await dbContext.AddRangeAsync(productList);
            await dbContext.SaveChangesAsync();

            var input = new PaginatedInPut(page, perPage, "", "", SearchOrder.Asc);

            var outPut = await repository.FilterPaginated(input);

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
            CleanInMemoryDatabase();
        }



        /*[Theory(DisplayName = nameof(ListProductOrderBY))]
        [Trait("Application-UseCase", "List Products")]
        [InlineData(SearchOrder.Asc)]
        [InlineData(SearchOrder.Desc)]
        public async void ListProductOrderBY(SearchOrder order)
        {
            var repository = new Mock<BusinessInterface.Repository.IProductRepository>();
            var notification = new Mock<BusinessInterface.INotification>();

            var productsFake = GetListProdutsConstant();

            var page = 1;
            var perPage = 2;
            var sort = "Name";

            var request = new ListProductInPut(page, perPage, "", sort, order);

            repository.Setup(r => r.FilterPaginated(It.IsAny<PaginatedInPut>()))
                .ReturnsAsync(new PaginatedOutPut<BusinessEntity.Product>(page, perPage, productsFake.Count(), productsFake));

            var useCase = new ApplicationUseCase.ListProducts(repository.Object, notification.Object);
            var outPut = await useCase.Handle(request);

            repository.Verify(r => r.FilterPaginated(It.IsAny<PaginatedInPut>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);

            Assert.NotNull(outPut);

            Assert.Equal(outPut.Page, page);
            Assert.Equal(outPut.PerPage, perPage);

            if (order == SearchOrder.Desc)
            {
                Assert.True(outPut.Itens.First().Name == fakeContantsNames.First());
                Assert.True(outPut.Itens.Last().Name == fakeContantsNames.Last());
            }
            else
            {
                Assert.True(outPut.Itens.First().Name == fakeContantsNames.Last());
                Assert.True(outPut.Itens.Last().Name == fakeContantsNames.First());
            }


        }*/
    }
}
