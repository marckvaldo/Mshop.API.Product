using MShop.Application.UseCases.Product.Common;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateStockProduct
{
    public class UpdateStockProducts : BaseUseCase, IUpdateStockProduct
    {
        private readonly IProductRepository _productRepository;

        public UpdateStockProducts(IProductRepository productRepository, INotification notification):base(notification)
            => _productRepository = productRepository;

        public async Task<ProductModelOutPut> Handler(UpdateStockProductInPut request)
        {
            var product = await _productRepository.GetById(request.Id);
            NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");

            product!.UpdateQuantityStock(request.Stock);
            product.IsValid(Notifications);
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
    }
}
