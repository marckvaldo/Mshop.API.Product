using MShop.Application.Common;
using MShop.Business.Enum.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.ListProducts
{
    public class ListProductInPut : PaginatedListInput
    {
        public ListProductInPut(int page, int perPage, string search, string sort, SearchOrder dir) : base(page, perPage, search, sort, dir)
        {
        }
    }
}
