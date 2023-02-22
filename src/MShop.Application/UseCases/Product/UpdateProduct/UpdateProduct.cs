﻿using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System.ComponentModel.DataAnnotations;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProduct : BaseUseCase, IUpdateProduct
    {
        private readonly IProductRepository _productRepository;

        public UpdateProduct(IProductRepository productRepository, INotification notification):base (notification)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductModelOutPut> Handle(UpdateProductInPut request)
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

            if (request.Imagem is not null && !string.IsNullOrWhiteSpace(request.Imagem))
                product.UpdateImage(request.Imagem);

            product.IsValid(_notifications);

            await _productRepository.Update(product);
            return new ProductModelOutPut(
                product.Id, 
                product.Description, 
                product.Name, 
                product.Price,
                product.Imagem, 
                product.Stock, 
                product.IsActive, 
                product.CategoryId);
            
        }
    }
}
