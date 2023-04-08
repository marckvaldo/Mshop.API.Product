using MShop.Application.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.ValueObject;
using Business = MShop.Business.Entity;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public class CreateProduct : BaseUseCase, ICreateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageRepository _imagesRepository;   
        private readonly IStorageService _storageService;

        public CreateProduct(IProductRepository productRepository, 
            INotification notification,
            ICategoryRepository categoryRepository,
            IStorageService storageService,
            IImageRepository imagesRepository) : base(notification)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _storageService = storageService;
            _imagesRepository = imagesRepository;
        }

        public async Task<ProductModelOutPut> Handler(CreateProductInPut request)
        {
            var product = new Business.Entity.Product(
                    request.Description,
                    request.Name,
                    request.Price,
                    request.CategoryId,
                    request.Stock,
                    request.IsActive
                );

            product.IsValid(_notifications);

            var hasCategory = await _categoryRepository.GetById(product.CategoryId);
            if (hasCategory is null)
            {
                Notify($"Categoria {product.Id} não encontrada");
                throw new ApplicationValidationException("");
            }

            await UploadImage(request, product);

            await _productRepository.Create(product);


            return new ProductModelOutPut(
                    product.Id,
                    product.Description,
                    product.Name,
                    product.Price,
                    product.Thumb?.Path,
                    product.Stock,
                    product.IsActive,
                    product.CategoryId
                );
        }

        private async Task UploadImage(CreateProductInPut request, Business.Entity.Product product)
        {
            if (request.Thumb is not null)
            {
                var thumbFile = Helpers.Base64ToStream(request.Thumb.FileStremBase64);
                var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{request.Thumb.Extension}", thumbFile.FileStrem);
                product.UpdateThumb(urlThumb);
            }
        }
    }
}
