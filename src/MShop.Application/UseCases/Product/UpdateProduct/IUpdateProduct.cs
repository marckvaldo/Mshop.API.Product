using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Product.UpdateProduct
{
    public interface IUpdateProduct : IRequestHandler<UpdateProductInPut, Result<ProductModelOutPut>>
    {
        public Task<Result<ProductModelOutPut>> Handle(UpdateProductInPut request, CancellationToken cancellationToken);
    }
}
