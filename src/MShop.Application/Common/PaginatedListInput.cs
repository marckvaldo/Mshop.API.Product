﻿using MShop.Business.Enum.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Common
{
    public abstract class PaginatedListInput
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public string Search { get;set; }

        public string Sort { get;set;}

        public SearchOrder Dir { get; set; }
        protected PaginatedListInput(int page, int perPage, string search, string sort = "", SearchOrder dir = SearchOrder.Asc)
        {
            Page = page;
            PerPage = perPage;
            Search = search;
            Sort = sort;
            Dir = dir;
        }

    }
}
