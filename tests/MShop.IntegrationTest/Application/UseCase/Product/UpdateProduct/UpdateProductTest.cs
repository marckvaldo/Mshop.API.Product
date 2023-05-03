using Microsoft.EntityFrameworkCore;
using Moq;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    [Collection("Update Products Collection")]
    [CollectionDefinition("Update Products Collection", DisableParallelization = true)]
    public class UpdateProductTest : UpdateProdutTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ImagesRepository _imageRepository;
        private readonly StorageService _storageService;

        public UpdateProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _categoryRepository = new CategoryRepository(_DbContext);
            _imageRepository = new ImagesRepository(_DbContext);
            _storageService = new StorageService();
        }

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task UpdateProduct()
        {
           
            var notificacao = new Notifications();

            var product = Faker();
            var request = RequestFake();
            

            await _DbContext.AddAsync(product);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.UpdateProduct(_repository, _categoryRepository, notificacao,_storageService);
            var outPut = await useCase.Handler(request);

            var productDb = await CreateDBContext(true).Products.Where(x=>x.Id == product.Id).FirstAsync();

            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(outPut.Name, productDb.Name);  
            Assert.Equal(outPut.Description, productDb.Description);  
            //Assert.Equal(outPut.Imagem, productDb.Thumb?.Path);
            Assert.Equal(outPut.Price, productDb.Price);  
            Assert.Equal(outPut.CategoryId, productDb.CategoryId);
            Assert.NotEmpty(outPut.Name);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
