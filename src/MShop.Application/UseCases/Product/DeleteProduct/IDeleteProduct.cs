using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public interface IDeleteProduct : IRequestHandler<DeleteProductInPut, Result<ProductModelOutPut>>
    {
        public Task<Result<ProductModelOutPut>> Handle(DeleteProductInPut request, CancellationToken cancellationToken);
    }
}
