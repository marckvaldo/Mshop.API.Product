using MediatR;
using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public interface IDeleteProduct : IRequestHandler<DeleteProductInPut, ProductModelOutPut>
    {
        public Task<ProductModelOutPut> Handle(DeleteProductInPut request, CancellationToken cancellationToken);
    }
}
