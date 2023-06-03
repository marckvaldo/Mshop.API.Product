using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Business.Entity;
using MShop.Repository.Context;
using MShop.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using InfraRepository = MShop.Repository.Repository;

namespace MShop.IntegrationTests.Repository.ImagenRepository
{
    public class ImageRepositoryTest : ImageRepositoryTestFixture
    {

        private readonly InfraRepository.ImagesRepository _imageRepository;
        private readonly RepositoryDbContext _DbContext;
        private readonly ImageRepositoryPersistence _persistence;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public ImageRepositoryTest()
        {
            _DbContext = CreateDBContext();
            _imageRepository = new InfraRepository.ImagesRepository(_DbContext);
            _persistence = new ImageRepositoryPersistence(_DbContext);

            var serviceColletion = new ServiceCollection();
            serviceColletion.AddLogging();
            var serviceProvider = serviceColletion.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_DbContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        }

        [Fact(DisplayName = nameof(CreateImage))]
        [Trait("Integration - Infra.Data", "Images Repositorio")]

        public async Task CreateImage()
        {
            var request = Faker();
            await _imageRepository.Create(request, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var outPut = await CreateDBContext(true).Images.FindAsync(request.Id);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.FileName, request.FileName);
            Assert.Equal(outPut.ProductId, request.ProductId);
        }


        [Fact(DisplayName = nameof(GetByIdImage))]
        [Trait("Integration - Infra.Data", "Images Repositorio")]

        public async void GetByIdImage()
        {
            var imageFaker = Faker();
            _persistence.Create(imageFaker);

            var outPut = await _imageRepository.GetById(imageFaker.Id);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.FileName, imageFaker.FileName);
            Assert.Equal(outPut.ProductId, imageFaker.ProductId);
        }


        [Fact(DisplayName = nameof(GetImages))]
        [Trait("Integration - Infra.Data", "Images Repositorio")]

        public async void GetImages()
        {
            var imagesFaker = FakerImages(40);
            _persistence.CreateList(imagesFaker);
            Assert.NotNull(imagesFaker);
            var imageFake = imagesFaker.FirstOrDefault();
            Assert.NotNull(imageFake);
            var outPut = await _imageRepository.Filter(x=>x.ProductId == imageFake.ProductId);

            Assert.NotNull(outPut);
            foreach(var item in outPut)
            {
                var image = imagesFaker.Where(x=>x.Id == item.Id).FirstOrDefault();

                Assert.NotNull(image);
                Assert.Equal(item.FileName, image.FileName);
                Assert.Equal(item.ProductId, image.ProductId);
            }
            
        }


        [Fact(DisplayName = nameof(DeleteImage))]
        [Trait("Integration - Infra.Data", "Images Repositorio")]

        public async void DeleteImage()
        {
            var quantity = 3;
            var imageFaker = FakerImages(quantity);
            _persistence.CreateList(imageFaker);
            var imageDelete = imageFaker.FirstOrDefault();
            Assert.NotNull(imageDelete);
            await _imageRepository.DeleteById(imageDelete, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            var outPut = await _persistence.GetAllImage();

            Assert.NotNull(outPut);
            Assert.Equal(quantity - 1, outPut.Count());
            Assert.Equal(0, outPut?.Where(x => x.Id == imageDelete.Id).Count());

        }


        public void Dispose()
        {
            CleanInMemoryDatabase();
        }

    }
}
