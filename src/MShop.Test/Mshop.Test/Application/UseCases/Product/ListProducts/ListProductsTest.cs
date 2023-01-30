using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Product.ListProducts;
using BusinessEntity = MShop.Business.Entity;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Enum.Paginated;
using MShop.Business.Paginated;

namespace Mshop.Tests.Application.UseCases.Product.ListProducts
{
    public class ListProductsTest : ListProductTestFixture
    {
        [Fact(DisplayName = nameof(ListProducts))]
        [Trait("Application-UseCase", "Create Products")]
        public async void ListProducts()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var productsFake = GetListProduts();

            var useCase = new ApplicationUseCase.ListProducts(repository.Object, notification.Object);

            var request = new ListProductInPut(
                            page:2,
                            perPage:10,
                            search: "search-exemple",
                            sort:"name",
                            dir:SearchOrder.Asc
                            );

            var outPutRepository = new PaginatedOutPut<BusinessEntity.Product>(
                                    currentPage: request.Page,
                                    perPage: request.PerPage,
                                    itens: productsFake,
                                    total: 10
                                    );

            repository.Setup(r => r.FilterPaginated(
                It.Is<PaginatedInPut>(
                    SearchInput => SearchInput.Page == request.Page
                    && SearchInput.PerPage == request.PerPage
                    && SearchInput.Search == request.Search
                    && SearchInput.OrderBy == request.Sort
                    && SearchInput.Order == request.Dir
                    )
                )).ReturnsAsync(outPutRepository);


            var outPut = await useCase.Handle(request);

            Assert.NotNull(outPut);
            Assert.Equal(productsFake.Count, outPut.Total);
            Assert.Equal(request.Page, outPut.Page);
            Assert.Equal(request.PerPage, outPut.PerPage);  

           


            /*Assert.Equal(outPut.Name, productFake.Name);
            Assert.Equal(outPut.Description, productFake.Description);
            Assert.Equal(outPut.Price, productFake.Price);
            Assert.Equal(outPut.Imagem, productFake.Imagem);
            Assert.Equal(outPut.CategoryId, productFake.CategoryId);
            Assert.Equal(outPut.Stock, productFake.Stock);
            Assert.Equal(outPut.IsActive, productFake.IsActive);*/


        }
    }
}
