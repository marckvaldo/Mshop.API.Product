using MediatR;
using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public interface IUpdateProduct : IRequestHandler<UpdateProductInPut, ProductModelOutPut>
    {
        public Task<ProductModelOutPut> Handle(UpdateProductInPut request, CancellationToken cancellationToken);
    }
}
