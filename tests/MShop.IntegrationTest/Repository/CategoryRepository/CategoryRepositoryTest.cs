using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Core.Enum.Paginated;
using MShop.Core.Paginated;
using MShop.Repository.Context;
using MShop.Repository.UnitOfWork;
using InfraRepository = MShop.Repository.Repository;

namespace MShop.IntegrationTests.Repository.CategoryRepository
{
    [Collection("Repository Category Collection")]
    [CollectionDefinition("Repository Category Collection", DisableParallelization = true)]
    public class CategoryRepositoryTest : CategoryRepositoryTestFixture
    {
        private readonly InfraRepository.CategoryRepository _categoryRepository;
        private readonly RepositoryDbContext _DbContext;
        private readonly CategoryRepositoryPertsistence _persistence;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public CategoryRepositoryTest()
        {
            _DbContext = CreateDBContext();
            _categoryRepository = new InfraRepository.CategoryRepository(_DbContext);
            _persistence = new CategoryRepositoryPertsistence(_DbContext);

            var serviceColletion = new ServiceCollection();
            serviceColletion.AddLogging();
            var serviceProvider = serviceColletion.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_DbContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]

        public async Task CreateCategory()
        {
            var request = Faker();
            await _categoryRepository.Create(request, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var newCategory = await CreateDBContext(true).Categories.FindAsync(request.Id);

            Assert.NotNull(newCategory);
            Assert.Equal(newCategory.Name, request.Name);
            Assert.Equal(newCategory.IsActive, request.IsActive);
        }


        [Fact(DisplayName = nameof(GetByIdCategory))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]

        public async void GetByIdCategory()
        {
            var categoryFaker = Faker();
            _persistence.Create(categoryFaker);

            var category = await _categoryRepository.GetById(categoryFaker.Id);

            Assert.NotNull(category);
            Assert.Equal(category.Name, categoryFaker.Name);
            Assert.Equal(category.IsActive, categoryFaker.IsActive);    
        }


        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]

        public async void UpdateProduct()
        {
            var categoryFaker = FakerCategories(3);
            _persistence.CreateList(categoryFaker);

            var category = categoryFaker.FirstOrDefault();
            Assert.NotNull(category);
            category.Update(faker.Commerce.Categories(1)[0]);
            category.Deactive();

            await _categoryRepository.Update(category, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var categoryDb = await _persistence.GetCategory(category.Id);

            Assert.NotNull(categoryDb);            
            Assert.Equal(categoryDb.Name, category.Name);
            Assert.Equal(categoryDb.IsActive, category.IsActive);

        }


        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]

        public async void DeleteProduct()
        {
            var quantity = 3;
            var categoryFaker = FakerCategories(quantity);
            _persistence.CreateList(categoryFaker);
            var categoryDelete = categoryFaker.FirstOrDefault();    
            Assert.NotNull(categoryDelete);
            await _categoryRepository.DeleteById(categoryDelete, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var outPut = await _persistence.GetAllCategories();

            Assert.NotNull(outPut);
            Assert.Equal(quantity - 1, outPut.Count());
            Assert.Equal(0, outPut?.Where(x => x.Id == categoryDelete.Id).Count());
            
        }

        [Fact(DisplayName = nameof(FilterPaginated))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]

        public async void FilterPaginated()
        {
            var quantity = 20;
            var perPage = 10;
            var categories = FakerCategories(quantity);
            _persistence.CreateList(categories);

            var request = new PaginatedInPut(1, perPage, "", "", SearchOrder.Desc);

            var outPut = await _categoryRepository.FilterPaginated(request);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut?.Itens);
            Assert.Equal(outPut.Total, quantity);
            Assert.Equal(outPut?.PerPage, perPage);
            Assert.Equal(outPut?.Itens.Count(), perPage);
            Assert.Equal(outPut?.CurrentPage, 1);

            foreach(var item in outPut?.Itens?.ToList())
            {
                var category = categories.Where(x => x.Id == item.Id).FirstOrDefault();
                Assert.NotNull(category);
                Assert.Equal(category.Name, item.Name);
                Assert.Equal(category.IsActive, item.IsActive);
            }

        }


        [Fact(DisplayName = nameof(SholdResultListEmptyFilterPaginated))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]
        public async Task SholdResultListEmptyFilterPaginated()
        {
            var perPage = 20;
            var input = new PaginatedInPut(1, perPage, "", "", SearchOrder.Asc);
            var outPut = await _categoryRepository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.True(outPut.Itens.Count == 0);
            Assert.True(outPut.Total == 0);
            Assert.Equal(input.PerPage, outPut.PerPage);
        }


        [Theory(DisplayName = nameof(SerachRestusPaginated))]
        [Trait("Integration - Infra.Data", "Category Repositorio")]
        [InlineData(10, 1, 10, 10)]
        [InlineData(17, 2, 10, 7)]
        [InlineData(17, 3, 10, 0)]

        public async Task SerachRestusPaginated(int quantityProduct, int page, int perPage, int expectedQuantityItems)
        {
            var categoryList = FakerCategories(quantityProduct);
            _persistence.CreateList(categoryList);

            var input = new PaginatedInPut(page, perPage, "", "", SearchOrder.Asc);
            var outPut = await _categoryRepository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Count == expectedQuantityItems);
            Assert.Equal(outPut.PerPage, perPage);
            Assert.True(outPut.Total == quantityProduct);
            Assert.Equal(input.PerPage, outPut.PerPage);

            foreach (var item in outPut.Itens)
            {
                var category = categoryList.Where(x => x.Id == item.Id).FirstOrDefault();
                Assert.NotNull(category);
                Assert.Equal(category.Name, item.Name);
                Assert.Equal(category.IsActive, item.IsActive);
            }

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }

    }
}
