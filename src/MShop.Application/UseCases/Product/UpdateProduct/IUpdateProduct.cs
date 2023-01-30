using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.UpdateProducts;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public interface IUpdateProduct
    {
        public Task<ProductModelOutPut> Handle(UpdateProductInPut request);
    }
}
