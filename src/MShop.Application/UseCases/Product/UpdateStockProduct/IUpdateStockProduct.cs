using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Product.UpdateStockProduct
{
    public interface IUpdateStockProduct : IRequestHandler<UpdateStockProductInPut, Result<ProductModelOutPut>>
    {
        public Task<Result<ProductModelOutPut>> Handle(UpdateStockProductInPut request, CancellationToken cancellationToken);
    }
}
