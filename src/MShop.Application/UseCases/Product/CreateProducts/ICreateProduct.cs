using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public interface ICreateProduct : IRequestHandler<CreateProductInPut, Result<ProductModelOutPut>>
    {
        public Task<Result<ProductModelOutPut>> Handle(CreateProductInPut categoryInput, CancellationToken cancellationToken);
    }
}
