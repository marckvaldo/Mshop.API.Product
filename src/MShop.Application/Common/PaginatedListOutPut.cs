using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Common
{
    public abstract class PaginatedListOutPut<TOutPutItens>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public IReadOnlyList<TOutPutItens> Itens { get; set; }
        protected PaginatedListOutPut(int page, int perPage, int total, IReadOnlyList<TOutPutItens> itens)
        {
            Page = page;
            PerPage = perPage;
            Total = total;
            Itens = itens;
        }

    }
}
