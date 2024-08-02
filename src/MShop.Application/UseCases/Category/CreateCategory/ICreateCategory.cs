using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInPut, Result<CategoryModelOutPut>>
    {
        Task<Result<CategoryModelOutPut>> Handle(CreateCategoryInPut request, CancellationToken cancellation);
    }
}
