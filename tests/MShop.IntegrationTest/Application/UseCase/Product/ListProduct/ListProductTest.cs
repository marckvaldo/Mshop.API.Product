using Moq;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Enum.Paginated;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.Business.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.ListProducts;
using BusinessEntity = MShop.Business.Entity;
using Microsoft.EntityFrameworkCore;

namespace MShop.IntegrationTests.Application.UseCase.Product.ListProduct
{
    public class ListProductTest : ListProductTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;

        public ListProductTest()
        {
            _DbContext = CreateDBContext(false, "ListProductTest");
            _repository = new ProductRepository(_DbContext);
        }

        [Fact(DisplayName = nameof(ListProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task ListProducts()
        {
           
            var notification = new Notifications();


            var productsFake = ListFake(20);
            await _DbContext.Products.AddRangeAsync(productsFake);
            await _DbContext.SaveChangesAsync();

          

            var useCase = new ListProducts(_repository, notification);

            var request = new ListProductInPut(
                            page: 1,
                            perPage:5,
                            search: "",
                            sort: "name",
                            dir: SearchOrder.Asc
                            );

            var outPut = await useCase.Handle(request);

            Assert.NotNull(outPut);
            Assert.Equal(productsFake.Count, outPut.Total);
            Assert.Equal(request.Page, outPut.Page);
            Assert.Equal(request.PerPage, outPut.PerPage);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Any());
        }

        public void Dispose()
        {
            CleanInMemoryDatabase(_DbContext);
        }
    }
}
