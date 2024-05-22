using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Business.Entity;
using MShop.Core.Enum.Paginated;
using MShop.Core.Message;
using MShop.Core.Paginated;
using MShop.IntegrationTests.Repository.CategoryRepository;
using MShop.Repository.Context;
using MShop.Repository.UnitOfWork;
using InfraRepository = MShop.Repository.Repository;

namespace MShop.IntegrationTests.Repository.ProductRepository
{
    [Collection("Repository Products Collection")]
    [CollectionDefinition("Repository Products Collection", DisableParallelization = true)]
    public class ProductRepositoryTest: ProductRespositoryTesteFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly InfraRepository.ProductRepository _repository;
        protected readonly ProductRepositoryPersistence _persistence;
        protected readonly CategoryRepositoryPertsistence _persistenceCategory;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public ProductRepositoryTest()
        {
            _DbContext = CreateDBContext();
            _repository = new InfraRepository.ProductRepository(_DbContext);
            _persistence = new ProductRepositoryPersistence(_DbContext);
            _persistenceCategory = new CategoryRepositoryPertsistence(_DbContext);

            var serviceColletion = new ServiceCollection();
            serviceColletion.AddLogging();
            var serviceProvider = serviceColletion.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_DbContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        }

        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task CreateProduct()
        {
            var product = Faker();
            await _repository.Create(product, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var newProduct = await CreateDBContext(true).Products.FindAsync(product.Id);

            Assert.NotNull(newProduct);
            Assert.Equal(product.Id, newProduct.Id);
            Assert.Equal(product.Name, newProduct.Name);
            Assert.Equal(product.Thumb, newProduct.Thumb);
            Assert.Equal(product.Price, newProduct.Price);
            Assert.Equal(product.Stock, newProduct.Stock);
            Assert.Equal(product.CategoryId, newProduct.CategoryId);
        }

        [Fact(DisplayName = nameof(GetByIdProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task GetByIdProduct()
        {

            var product = Faker();
            var productList = FakerList(20);
            
            productList.Add(product);
            _persistence.CreateList(productList);
            var outPut = await _repository.GetById(product.Id);

            Assert.NotNull(outPut);
            Assert.Equal(product.Id, outPut.Id);
            Assert.Equal(product.Name, outPut.Name);
            Assert.Equal(product.Thumb, outPut.Thumb);
            Assert.Equal(product.Price, outPut.Price);
            Assert.Equal(product.Stock, outPut.Stock);
            Assert.Equal(product.CategoryId, outPut.CategoryId);
        }

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task UpdateProduct()
        {
            var notification = new Notifications() ;
            var repository = new InfraRepository.ProductRepository(_DbContext);
            var request = FakerImage();
            var productList = FakerList(20);
            _persistence.CreateList(productList);
            
            Guid id = productList.First().Id;
            var product = await _repository.GetById(id);

            Assert.NotNull(product);

            product.Update(request.Description, request.Name, request.Price, request.CategoryId);
            product.UpdateThumb(request.Thumb.Path);
            product.UpdateQuantityStock(request.Stock);
            product.IsValid(notification);

            await repository.Update(product, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var productUpdate = await (CreateDBContext(true)).Products.FindAsync(id);   

            Assert.NotNull(productUpdate);
            Assert.Equal(id, productUpdate.Id);
            Assert.Equal(request.Name, productUpdate.Name);
            Assert.Equal(request.Thumb.Path, productUpdate.Thumb.Path);
            Assert.Equal(request.Price, productUpdate.Price);
            Assert.Equal(request.Stock, productUpdate.Stock);
            Assert.Equal(request.CategoryId, productUpdate.CategoryId);
        }


        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task DeleteProduct()
        {
            var productList = FakerList(20);
            _persistence.CreateList(productList);   

            var request = productList.First();
            await _repository.DeleteById(request, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var productUpdate = await (CreateDBContext(true)).Products.FindAsync(request.Id);

            Assert.Null(productUpdate);
        }


        [Fact(DisplayName = nameof(FilterPaginated))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]
        public async Task FilterPaginated()
        {
            var category = FakerCategory();
            _persistenceCategory.Create(category);

            var productList = FakerList(category,20);
            _persistence.CreateList(productList);

            var perPage = 10;
            var input = new PaginatedInPut(1, perPage, "","",SearchOrder.Asc);
            var outPut = await _repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Count == perPage);
            Assert.Equal(input.PerPage, outPut.PerPage);

            foreach(Product item in outPut.Itens)
            {
                var product = productList.Find(x => x.Id == item.Id);
                Assert.NotNull(product);
                Assert.Equal(product.Name, item.Name);
                Assert.Equal(product.Description, item.Description);
                Assert.Equal(product.Price, item.Price);
                Assert.Equal(product.CategoryId, item.CategoryId);
            }

        }


        [Fact(DisplayName = nameof(SholdResultListEmptyFilterPaginated))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]

        public async Task SholdResultListEmptyFilterPaginated()
        {
            var perPage = 20;
            var input = new PaginatedInPut(1, perPage, "", "", SearchOrder.Asc);
            var outPut = await _repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.True(outPut.Itens.Count == 0);
            Assert.True(outPut.Total == 0);
            Assert.Equal(input.PerPage, outPut.PerPage);
        }


        [Theory(DisplayName = nameof(SerachRestusPaginated))]
        [Trait("Integration - Infra.Data", "Product Repositorio")]
        [InlineData(10,1,10,10)]
        [InlineData(17, 2, 10,7)]
        [InlineData(17, 3, 10, 0)]

        public async Task SerachRestusPaginated(int quantityProduct, int page, int perPage, int expectedQuantityItems)
        {

            var category = FakerCategory();
            _persistenceCategory.Create(category);

            var productList = FakerList(category,quantityProduct);
            _persistence.CreateList(productList);

            var input = new PaginatedInPut(page, perPage, "", "", SearchOrder.Asc);
            var outPut = await _repository.FilterPaginated(input);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Count == expectedQuantityItems);
            Assert.Equal(outPut.PerPage, perPage);   
            Assert.True(outPut.Total == quantityProduct);
            Assert.Equal(input.PerPage, outPut.PerPage);

            foreach (Product item in outPut.Itens)
            {
                var product = productList.Find(x => x.Id == item.Id);
                Assert.NotNull(product);
                Assert.Equal(product.Name, item.Name);
                Assert.Equal(product.Description, item.Description);
                Assert.Equal(product.Price, item.Price);
                Assert.Equal(product.CategoryId, item.CategoryId);
            }

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
