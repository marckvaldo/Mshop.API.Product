using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Interface;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Enum.Paginated;
using ApplicationUseCase = MShop.Application.UseCases.images.ListImage;

namespace MShop.IntegrationTests.Application.UseCase.Images.ListImage
{
    [Collection("List Image Collection")]
    [CollectionDefinition("List Image Collection", DisableParallelization = true)]
    public class ListImageTest : ImageTestFixture, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;

        public ListImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
        }

        [Fact(DisplayName = nameof(ListImage))]
        [Trait("Integration-Application", "Image Use Case")]
        public async Task ListImage()
        {
            var quantity = 20;
            var productId = Guid.NewGuid();
            var imagesFake = FakerList(productId, quantity);
            await _imagePersistense.CreateList(imagesFake);
         
            var useCase = new ApplicationUseCase.ListImage(_notification, _imageRepository);
            var outPut = await useCase.Handler(productId);

            Assert.NotNull(outPut);
            Assert.Equal(imagesFake.Count, quantity);
            Assert.Equal(outPut.Images.Count, quantity);
            foreach (var item in outPut.Images) 
            {
                var image = outPut.Images.Where(i => i.Image == item.Image);
                Assert.NotNull(image);
            }
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
