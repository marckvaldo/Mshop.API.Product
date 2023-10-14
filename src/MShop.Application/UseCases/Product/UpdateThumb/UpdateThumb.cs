using MShop.Application.Common;
using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Business.Exception;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateThumb
{
    public class UpdateThumb : BaseUseCase, IUpdateThumb
    {
        private readonly IProductRepository _productRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateThumb(
            IProductRepository productRepository, 
            IStorageService storageService,
            INotification notification,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _productRepository = productRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModelOutPut> Handle(UpdateThumbInPut request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.Id);
            NotifyExceptionIfNull(product, "Não foi possivel localizar o produto!");

            product!.IsValid(Notifications);
            product.ProductUpdatedEvent();
            NotifyExceptionIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductUpdatedEvent");

            await UploadImage(request, product);
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
                product.IsSale
                );

        }

        private async Task UploadImage(UpdateThumbInPut request, Business.Entity.Product product)
        {
            if (string.IsNullOrEmpty(request.Thumb?.FileStremBase64.Trim()))
                return;

            var thumb = Helpers.Base64ToStream(request.Thumb.FileStremBase64);
            var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{thumb.Extension}", thumb.FileStrem);

            if (!string.IsNullOrEmpty(request.Thumb.FileStremBase64.Trim()) && !string.IsNullOrEmpty(product.Thumb?.Path.Trim()))
                await _storageService.Delete(product.Thumb.Path);

            product.UpdateThumb(urlThumb);

        }
    }
}
