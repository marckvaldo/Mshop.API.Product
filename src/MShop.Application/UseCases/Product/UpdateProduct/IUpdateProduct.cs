using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public interface IUpdateProduct
    {
        public Task<ProductModelOutPut> Handler(UpdateProductInPut request);
    }
}
