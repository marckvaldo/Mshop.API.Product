using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Enum.Paginated;
using MShop.Business.Interface;
using MShop.Business.Validation;
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

        public ListProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _productPersistence = new ProductPersistence(_DbContext);
            _notification = new Notifications();
        }

        [Fact(DisplayName = nameof(ListProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task ListProducts()
        {
           
            //var notification = new Notifications();

            var productsFake = ListFake(20);
            await _productPersistence.CreateList(productsFake);
            //await _DbContext.Products.AddRangeAsync(productsFake);
            //await _DbContext.SaveChangesAsync();
           

            var useCase = new ListProducts(_repository, _notification);
            var request = new ListProductInPut(
                            page: 1,
                            perPage:5,
                            search: "",
                            sort: "name",
                            dir: SearchOrder.Asc
                            );

            var outPut = await useCase.Handler(request);

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
