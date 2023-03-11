using MShop.Business.Enum.Paginated;
using MShop.Business.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public class ListCategoryInPut : PaginatedInPut
    {
        public ListCategoryInPut(int page, int perPage, string search, string sort, SearchOrder dir) : base(page, perPage, search, sort, dir)
        {

        }
    }
}
