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
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStockProducts(
            IProductRepository productRepository, 
            INotification notification,
            IUnitOfWork unitOfWork):base(notification)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModelOutPut> Handler(UpdateStockProductInPut request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.Id);
            NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");

            product!.UpdateQuantityStock(request.Stock);
            product.IsValid(Notifications);
            product.ProductUpdatedEvent();
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
                product.CategoryId);
        }
    }
}
