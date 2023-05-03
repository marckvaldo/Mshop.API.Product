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
        public UpdateThumb(IProductRepository productRepository, IStorageService storageService, INotification notification) : base(notification)
        {
            _productRepository = productRepository;
            _storageService = storageService;
        }

        public async Task<ProductModelOutPut> Handler(UpdateThumbInput request)
        {
            var product = await _productRepository.GetById(request.Id);
            NotFoundException.ThrowIfnull(product, "Não foi possivel localizar o produto!");

            product.IsValid(Notifications);

            await UploadImage(request, product);
            await _productRepository.Update(product);

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
                product.IsPromotion
                );

        }

        private async Task UploadImage(UpdateThumbInput request, Business.Entity.Product product)
        {
            if (string.IsNullOrEmpty(request.Thumb?.FileStremBase64.Trim()))
                return;

            var thumb = Helpers.Base64ToStream(request.Thumb.FileStremBase64);
            var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{thumb.Extension}", thumb.FileStrem);
            product.UpdateThumb(urlThumb);

        }
    }
}
