using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Events.Products;
using MShop.Core.Base;
using MShop.Core.Data;
using MShop.Core.DomainObject;
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

        public async Task<Result<ProductModelOutPut>> Handle(UpdateStockProductInPut request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.Id);
            //NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");
            if (NotifyErrorIfNull(product, "Não foi possivel localizar a produto da base de dados!"))
                return Result<ProductModelOutPut>.Error();

            product!.UpdateQuantityStock(request.Stock);

            if(!product.IsValid(Notifications))
                return Result<ProductModelOutPut>.Error();

           
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

            if(NotifyErrorIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductUpdatedEvent"))
                return Result<ProductModelOutPut>.Error();

            await _unitOfWork.CommitAsync(cancellationToken);

            var produtoOutPut =  new ProductModelOutPut(
                product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Thumb?.Path,
                product.Stock,
                product.IsActive,
                product.CategoryId);

            return Result<ProductModelOutPut>.Success(produtoOutPut);
        }
    }
}
