using MShop.Application.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Exception;
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
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;        

        public CreateProduct(IProductRepository productRepository, 
            INotification notification,
            ICategoryRepository categoryRepository,
            IStorageService storageService,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _storageService = storageService;  
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModelOutPut> Handler(CreateProductInPut request, CancellationToken cancellationToken)
        {
            var product = new Business.Entity.Product(
                    request.Description,
                    request.Name,
                    request.Price,
                    request.CategoryId,
                    request.Stock,
                    request.IsActive
                );

            product.IsValid(Notifications);

            var hasCategory = await _categoryRepository.GetById(product.CategoryId);
            NotifyExceptionIfNull(hasCategory, $"Categoria {product.CategoryId} não encontrada");

            try
            {
                await UploadImage(request, product);
                await _productRepository.Create(product,cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);


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
            catch(Exception)
            {
                if(product?.Thumb?.Path is not null) 
                    await _storageService.Delete(product.Thumb.Path);
                throw;
            }
        }

        private async Task UploadImage(CreateProductInPut request, Business.Entity.Product product)
        {
            if (string.IsNullOrEmpty(request.Thumb?.FileStremBase64.Trim()))
                return;
           

            var thumbFile = Helpers.Base64ToStream(request.Thumb.FileStremBase64);
            var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{thumbFile.Extension}", thumbFile.FileStrem);
            product.UpdateThumb(urlThumb);
            
        }
    }
}
