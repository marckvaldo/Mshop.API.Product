using MShop.Application.UseCases.Category.ListCategorys;
using MShop.Core.Enum.Paginated;
using MShop.Core.Message;
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Category.ListCategorys;

namespace MShop.IntegrationTests.Application.UseCase.Category.ListCategory
{
    [Collection("List Category Collection")]
    [CollectionDefinition("List Category Collection", DisableParallelization = true)]
    public class ListCategoryTest : CategoryTestFixture, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _context;
        private readonly CategoryPersistence _categoryPersistence;

        public ListCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
            _productRepository = new ProductRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);
        }


        [Fact(DisplayName = nameof(ListCatgories))]
        [Trait("Integration-Application", "Category Use Case")]

        public async Task ListCatgories()
        {

            var categoryFake = FakerList(20);
            await _categoryPersistence.CreateList(categoryFake);

            var useCase = new ApplicationUseCase.ListCategory(_categoryRepository, _notification);
            var request = new ListCategoryInPut(
                            page: 1,
                            perPage: 5,
                            search: "",
                            sort: "name",
                            dir: SearchOrder.Asc
                            );

            var outPut = await useCase.Handle(request, CancellationToken.None);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(categoryFake.Count, result.Total);
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
