using MediatR;
using MShop.Application.UseCases.Category.Common;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInPut, CategoryModelOutPut>
    {
        Task<CategoryModelOutPut> Handle(CreateCategoryInPut request, CancellationToken cancellation);
    }
}
