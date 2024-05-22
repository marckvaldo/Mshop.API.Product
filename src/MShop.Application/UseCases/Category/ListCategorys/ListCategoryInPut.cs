using MediatR;
using MShop.Application.Common;
using MShop.Core.Enum.Paginated;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public class ListCategoryInPut : PaginatedListInput, IRequest<ListCategoryOutPut>
    {
        public ListCategoryInPut(
            int page, 
            int perPage, 
            string search, 
            string sort, 
            SearchOrder dir) 
            : base(page, perPage, search, sort, dir)
        {

        }

        public ListCategoryInPut() : base(1, 15, "", "", SearchOrder.Asc)
        {

        }
    }
}
