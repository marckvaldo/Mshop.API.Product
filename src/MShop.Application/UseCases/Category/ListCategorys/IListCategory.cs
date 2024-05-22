using MediatR;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public interface IListCategory : IRequestHandler<ListCategoryInPut, ListCategoryOutPut>
    {
        Task<ListCategoryOutPut> Handle(ListCategoryInPut request, CancellationToken cancellation);
    }
}
