using MediatR;
using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.UpdateStockProduct
{
    public interface IUpdateStockProduct : IRequestHandler<UpdateStockProductInPut, ProductModelOutPut>
    {
        public Task<ProductModelOutPut> Handle(UpdateStockProductInPut request, CancellationToken cancellationToken);
    }
}
