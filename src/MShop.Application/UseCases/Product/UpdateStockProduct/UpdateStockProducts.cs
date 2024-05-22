using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Events.Products;
using MShop.Core.Base;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Repository.Interface;

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

        public async Task<ProductModelOutPut> Handle(UpdateStockProductInPut request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.Id);
            NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");

            product!.UpdateQuantityStock(request.Stock);
            product.IsValid(Notifications);

           
            await _productRepository.Update(product, cancellationToken);

            product.ProductUpdatedEvent(new ProductUpdatedEvent(
                    product.Id,
                    product.Description,
                    product.Name,
                    product.Price,
                    product.Stock,
                    product.IsActive,
                    product.CategoryId,
                    "Cantegoria",
                    product.Thumb?.Path,
                    product.IsSale));

            NotifyExceptionIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductUpdatedEvent");

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
