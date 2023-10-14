using MediatR;
using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public interface ICreateProduct : IRequestHandler<CreateProductInPut, ProductModelOutPut>
    {
        public Task<ProductModelOutPut> Handle(CreateProductInPut categoryInput, CancellationToken cancellationToken);
    }
}
