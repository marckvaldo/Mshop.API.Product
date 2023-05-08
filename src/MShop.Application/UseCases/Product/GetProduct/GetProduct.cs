using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public class GetProduct : BaseUseCase, IGetProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;

        public GetProduct(IProductRepository productRepository, IImageRepository imageRepository, INotification notification) : base(notification)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
        }

        public async Task<GetProductOutPut> Handler(Guid Id)
        {
            var product = await _productRepository.GetProductWithCategory(Id);


            //NotFoundException.ThrowIfnull(product, "Não foi possivel localizar a produto da base de dados!");
            NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");

            var images = await _imageRepository.Filter(x => x.ProductId == product.Id);

            //implementar o delete de images
            return new GetProductOutPut(
                product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Thumb?.Path,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                (new CategoryModelOutPut(product.CategoryId, product.Category.Name, product.Category.IsActive)),
                images.Select(x => x?.FileName).ToList(),
                product.IsPromotion) ;
        }
    }
}
