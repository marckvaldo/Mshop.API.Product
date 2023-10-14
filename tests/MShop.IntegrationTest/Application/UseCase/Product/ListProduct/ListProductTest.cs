using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Enum.Paginated;
using MShop.Business.Interface;
using MShop.Business.Validation;
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

            Assert.NotNull(outPut);
            Assert.Equal(productsFake.Count, outPut.Total);
            Assert.Equal(request.Page, outPut.Page);
            Assert.Equal(request.PerPage, outPut.PerPage);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Any());
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
