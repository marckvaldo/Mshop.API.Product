using MShop.Business.Enum.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Paginated
{
    public class PaginatedInPut
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public string Search { get; set; }

        public string OrderBy { get; set; }

        public SearchOrder Order { get; set; }

        public PaginatedInPut(int page = 1, int perPage = 20, string search = "", string orderBy = "", SearchOrder order = SearchOrder.Asc)
        {
            Page = page;
            PerPage = perPage;
            Search = search;
            OrderBy = orderBy;
            Order = order;
        }

    }
}
