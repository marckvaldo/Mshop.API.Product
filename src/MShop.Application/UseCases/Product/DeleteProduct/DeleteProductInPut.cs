using MediatR;
using MShop.Application.UseCases.Product.Common;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProductInPut : IRequest<ProductModelOutPut>
    {
        public DeleteProductInPut(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
