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

namespace MShop.IntegrationTests.Application.UseCase.Product.ListProduct
{
    public class ListProductTest : ListProductTestFixture
    {
        [Fact(DisplayName = nameof(ListProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async void ListProducts()
        {
            RepositoryDbContext DbContext = CreateDBContext();

            var repository = new ProductRepository(DbContext);
            var notification = new Notifications();

            var productsFake = ListFake(20);
            await DbContext.Products.AddRangeAsync(productsFake);
            await DbContext.SaveChangesAsync();

            var useCase = new ListProducts(repository, notification);

            var random = new Random();

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

    }
}
