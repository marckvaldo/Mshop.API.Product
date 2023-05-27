using MShop.Application.UseCases.Category.Common;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory
    {
        Task<CategoryModelOutPut> Handler(UpdateCategoryInPut request, CancellationToken cancellationToken);
    }
}
