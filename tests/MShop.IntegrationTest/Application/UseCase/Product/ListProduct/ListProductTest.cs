using MShop.Application.UseCases.Product.ListProducts;
using MShop.Core.Enum.Paginated;
using MShop.Core.Message;
using MShop.IntegrationTests.Application.UseCase.Category;
using MShop.Repository.Context;
using MShop.Repository.Repository;

namespace MShop.IntegrationTests.Application.UseCase.Product.ListProduct
{
    [Collection("List Products Collection")]
    [CollectionDefinition("List Products Collection", DisableParallelization = true)]
    public class ListProductTest : ListProductTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly INotification _notification;
        private readonly ProductPersistence _productPersistence;
        private readonly CategoryPersistence _categoryPersistence;

        public ListProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _productPersistence = new ProductPersistence(_DbContext);
            _categoryPersistence = new CategoryPersistence(_DbContext);
            _notification = new Notifications();

        }

        [Fact(DisplayName = nameof(ListProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task ListProducts()
        {
            var category = FakeCategory();
            await _categoryPersistence.Create(category);

            var productsFake = ListFake(category,20);
            await _productPersistence.CreateList(productsFake);

            var useCase = new ListProducts(_repository, _notification);
            var request = new ListProductInPut(
                            page: 1,
                            perPage:5,
                            search: "",
                            sort: "name",
                            dir: SearchOrder.Asc
                            );

            var outPut = await useCase.Handle(request, CancellationToken.None);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(productsFake.Count, result.Total);
            Assert.Equal(request.Page, result.Page);
            Assert.Equal(request.PerPage, result.PerPage);
            Assert.NotNull(result.Itens);
            Assert.True(result.Itens.Any());
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
