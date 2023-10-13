using MediatR;
using MShop.Application.UseCases.Category.Common;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory : IRequestHandler<UpdateCategoryInPut, CategoryModelOutPut>
    {
        Task<CategoryModelOutPut> Handle(UpdateCategoryInPut request, CancellationToken cancellationToken);
    }
}
