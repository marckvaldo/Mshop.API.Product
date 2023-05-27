using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.UpdateStockProduct
{
    public interface IUpdateStockProduct
    {
        public Task<ProductModelOutPut> Handler(UpdateStockProductInPut request, CancellationToken cancellationToken);
    }
}
