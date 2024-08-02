using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProductInPut : IRequest<Result<ProductModelOutPut>>
    {
        public DeleteProductInPut(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
