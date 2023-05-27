using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public interface ICreateProduct
    {
        public Task<ProductModelOutPut> Handler(CreateProductInPut categoryInput, CancellationToken cancellationToken);
    }
}
