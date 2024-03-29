﻿using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProduct : BaseUseCase, IDeleteProduct
    {
        private readonly IProductRepository _productRespository;
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
 
        public DeleteProduct(IProductRepository productRespository, 
            IImageRepository imageRepository,
            INotification notification,
            IStorageService storageService,
            IUnitOfWork unitOfWork):base(notification)
        {
            _productRespository = productRespository;
            _imageRepository = imageRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModelOutPut> Handle(DeleteProductInPut request, CancellationToken cancellationToken)
        {
            var product = await _productRespository.GetById(request.Id);
            NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");

            var hasImages = await _imageRepository.Filter(x => x.ProductId == product!.Id);
            if(hasImages?.Count > 0)
                NotifyException("Existe(m) Imagen(s) associada(s) a esse produto");

            product!.ProductRemovedEvent();
            NotifyExceptionIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductDeletedEvent");

            await _productRespository.DeleteById(product, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);   

            if (product?.Thumb?.Path is not null) await _storageService.Delete(product.Thumb.Path);

            /*return new ProductModelOutPut(
                product!.Id, 
                product.Description,
                product.Name, 
                product.Price, 
                product.Thumb?.Path, 
                product.Stock, 
                product.IsActive, 
                product.CategoryId);*/

            return ProductModelOutPut.FromProduct(product); 
        }
    }
}
