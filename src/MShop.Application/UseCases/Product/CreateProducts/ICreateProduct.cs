using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public interface ICreateProduct
    {
        public Task<ProductModelOutPut> Handle(CreateProductInPut categoryInput);
    }
}
