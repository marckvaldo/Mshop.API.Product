using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory : IRequestHandler<UpdateCategoryInPut, Result<CategoryModelOutPut>>
    {
        Task<Result<CategoryModelOutPut>> Handle(UpdateCategoryInPut request, CancellationToken cancellationToken);
    }
}
