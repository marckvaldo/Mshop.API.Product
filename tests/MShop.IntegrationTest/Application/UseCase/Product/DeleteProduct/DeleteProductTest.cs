using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.DeleteProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.DeleteProduct
{
    [Collection("Delete Products Collection")]
    [CollectionDefinition("Delete Products Collection", DisableParallelization = true)]
    public class DeleteProductTest : DeleteProductTestFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly ImagesRepository _imagesRepository;

        public DeleteProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _imagesRepository = new ImagesRepository(_DbContext);   
        }

        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task DeleteProduct()
        {
          
            var notification = new Notifications();

            var product = Faker();
            await _DbContext.AddAsync(product);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.DeleteProduct(_repository, _imagesRepository, notification);
            await useCase.Handle(product.Id);

            var productDbDelete = await CreateDBContext(true).Products.FindAsync(product.Id);

            Assert.Null(productDbDelete);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
