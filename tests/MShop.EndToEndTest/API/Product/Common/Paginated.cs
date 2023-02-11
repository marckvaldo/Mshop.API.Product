using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Product.Common
{
    public class Paginated<TResult>
    {
        public Paginated(int page, int perPage, int total, List<TResult> itens)
        {
            Page = page;
            PerPage = perPage;
            Total = total;
            Itens = itens;
        }

        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public List<TResult> Itens { get; set; }
    }
}
