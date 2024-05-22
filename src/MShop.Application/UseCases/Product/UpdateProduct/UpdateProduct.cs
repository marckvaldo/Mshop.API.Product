using MShop.Application.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Events.Products;
using MShop.Business.Interface.Service;
using MShop.Core.Base;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProduct : BaseUseCase, IUpdateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly IStorageService _storageService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProduct(IProductRepository productRepository, 
            ICategoryRepository categoryRepository,
            INotification notification,
            IStorageService storageService,
            IUnitOfWork unitOfWork
            ) :base (notification)
        {
            _productRepository = productRepository;
            _storageService = storageService;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModelOutPut> Handle(UpdateProductInPut request, CancellationToken cancellationToken)
        {            
            var product = await _productRepository.GetById(request.Id);
            NotFoundException.ThrowIfnull(product, "Não foi possivel localizar a produto da base de dados!");

            product!.Update(request.Description, request.Name, request.Price, request.CategoryId);

            if (request.IsActive)
                product.Activate();
            else
                product.Deactive();

            if (request.IsPromotion)
                product.ActivateSale();
            else
                product.DeactiveSale();
            
            product.IsValid(Notifications);
      
            var category = await _categoryRepository.GetById(product.CategoryId);
            NotFoundException.ThrowIfnull(category, $"Categoria {product.CategoryId} não encontrada");

            try
            {
                await UploadImage(request, product);

                product.ProductUpdatedEvent(new ProductUpdatedEvent(
                    product.Id,
                    product.Description,
                    product.Name,
                    product.Price,
                    product.Stock,
                    product.IsActive,
                    product.CategoryId,
                    category.Name,
                    product.Thumb?.Path,
                    product.IsSale));

                NotifyExceptionIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductUpdatedEvent");

                await _productRepository.Update(product, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);   

                return new ProductModelOutPut(
                    product.Id,
                    product.Description,
                    product.Name,
                    product.Price,
                    product.Thumb?.Path,
                    product.Stock,
                    product.IsActive,
                    product.CategoryId,
                    null,
                    product.IsSale);
            }
            catch(Exception)
            {
                if (product?.Thumb?.Path is not null) await _storageService.Delete(product.Thumb.Path);
                throw;
            }
            
        }

        private async Task UploadImage(UpdateProductInPut request, Business.Entity.Product product)
        {
            if (string.IsNullOrEmpty(request.Thumb?.FileStremBase64.Trim()))
                return;                   

            var thumb = Helpers.Base64ToStream(request.Thumb!.FileStremBase64);
            var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{thumb.Extension}", thumb.FileStrem);

            if (!string.IsNullOrEmpty(product.Thumb?.Path.Trim()) && !string.IsNullOrEmpty(request.Thumb?.FileStremBase64.Trim()))
                await _storageService.Delete(product.Thumb.Path);

            product.UpdateThumb(urlThumb);
        }
    }
}
