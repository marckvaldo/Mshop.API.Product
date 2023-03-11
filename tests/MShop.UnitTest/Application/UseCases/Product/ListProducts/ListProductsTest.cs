using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Product.ListProducts;
using BusinessEntity = MShop.Business.Entity;
using BusinessInterface = MShop.Business.Interface;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Enum.Paginated;
using MShop.Business.Paginated;

namespace Mshop.Tests.Application.UseCases.Product.ListProducts
{
    public class ListProductsTest : ListProductTestFixture
    {
        [Fact(DisplayName = nameof(ListProducts))]
        [Trait("Application-UseCase", "List Products")]
        public async void ListProducts()
        {
            var repository = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();

            var productsFake = GetListProduts();

            var useCase = new ApplicationUseCase.ListProducts(repository.Object, notification.Object);

            var random = new Random();

            var request = new ListProductInPut(
                            page: random.Next(1,10),
                            perPage: random.Next(10,20),
                            search: faker.Commerce.ProductName(),
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


            var outPut = await useCase.Handler(request);

            Assert.NotNull(outPut);
            Assert.Equal(productsFake.Count, outPut.Total);
            Assert.Equal(request.Page, outPut.Page);
            Assert.Equal(request.PerPage, outPut.PerPage);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Any());
        }

    }
}
