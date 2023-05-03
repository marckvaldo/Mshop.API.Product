using MShop.Application.Common;
using MShop.Application.UseCases.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.ListProducts
{
    public class ListProductsOutPut : PaginatedListOutPut<ProductModelOutPut>
    {
        public ListProductsOutPut(int page, int perPage, int total, IReadOnlyList<ProductModelOutPut> itens) 
            : base(page, perPage, total, itens)
        {

        }
    }
}
