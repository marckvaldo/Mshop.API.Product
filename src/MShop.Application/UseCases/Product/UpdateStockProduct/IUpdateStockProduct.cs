using MShop.Application.Common;

namespace MShop.Application.UseCases.Product.UpdateStockProduct
{
    public interface IUpdateStockProduct
    {
        public Task<ProductModelOutPut> Handle(UpdateStockProductInPut request);
    }
}
