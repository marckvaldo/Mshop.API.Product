using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System.ComponentModel.DataAnnotations;
using MShop.Business.ValueObject;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Interface.Service;
using MShop.Application.Common;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProduct : BaseUseCase, IUpdateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly IStorageService _storageService;

        public UpdateProduct(IProductRepository productRepository, 
            INotification notification,
            IStorageService storageService) :base (notification)
        {
            _productRepository = productRepository;
            _storageService = storageService;
        }

        public async Task<ProductModelOutPut> Handler(UpdateProductInPut request)
        {            
            var product = await _productRepository.GetById(request.Id);

            if(product == null)
            {
                Notify("Não foi possivel localizar o produto na base de dados");
                throw new ApplicationValidationException("");
            }


            product.Update(request.Description, request.Name, request.Price, request.CategoryId);
            
            if (request.IsActive)
                product.Activate();
            else
                product.Deactive();

            product.IsValid(_notifications);

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
                product.CategoryId);
            
        }

        private async Task UploadImage(UpdateProductInPut request, Business.Entity.Product product)
        {
            if (request.Thumb is not null)
            {
                var thumb = Helpers.Base64ToStream(request.Thumb.FileStremBase64);
                var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{thumb.Extension}", thumb.FileStrem);
                product.UpdateThumb(urlThumb);
            }
        }
    }
}
