using MediatR;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public interface IListCategory : IRequestHandler<ListCategoryInPut, Result<ListCategoryOutPut>>
    {
        Task<Result<ListCategoryOutPut>> Handle(ListCategoryInPut request, CancellationToken cancellation);
    }
}
