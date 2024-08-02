using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Category.DeleteCategory
{
    public interface IDeleteCategory : IRequestHandler<DeleteCategoryInPut, Result<CategoryModelOutPut>>
    {
        Task<Result<CategoryModelOutPut>> Handle(DeleteCategoryInPut request, CancellationToken cancellationToken);
    }
}
