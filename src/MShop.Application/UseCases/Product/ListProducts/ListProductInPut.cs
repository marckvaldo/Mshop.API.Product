using MediatR;
using MShop.Application.Common;
using MShop.Core.Enum.Paginated;


namespace MShop.Application.UseCases.Product.ListProducts
{
    public class ListProductInPut : PaginatedListInput, IRequest<ListProductsOutPut>
    {
        public ListProductInPut(int page, int perPage, string search, string sort, SearchOrder dir) : base(page, perPage, search, sort, dir)
        {

        }

        public ListProductInPut() : base(1, 15, "", "", SearchOrder.Asc)
        {

        }
    }
}
