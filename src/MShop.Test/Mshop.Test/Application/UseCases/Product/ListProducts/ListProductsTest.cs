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


            var outPut = await useCase.Handle(request);

            Assert.NotNull(outPut);
            Assert.Equal(productsFake.Count, outPut.Total);
            Assert.Equal(request.Page, outPut.Page);
            Assert.Equal(request.PerPage, outPut.PerPage);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Any());
        }


        [Theory(DisplayName = nameof(ListProductOrderBY))]
        [Trait("Application-UseCase", "List Products")]
        [InlineData(SearchOrder.Asc)]
        [InlineData(SearchOrder.Desc)]
        public async void ListProductOrderBY( SearchOrder order)
        {
            var repository = new Mock<BusinessInterface.Repository.IProductRepository>();
            var notification = new Mock<BusinessInterface.INotification>();

            var productsFake = GetListProdutsConstant();

            var page = 1;
            var perPage = 2;
            var sort = "Name";

            var request = new ListProductInPut(page, perPage, "", sort, order);

            repository.Setup(r => r.FilterPaginated(It.IsAny<PaginatedInPut>()))
                .ReturnsAsync(new PaginatedOutPut<BusinessEntity.Product>(page, perPage, productsFake.Count(),productsFake));

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
                

        }
    }
}
