using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.UpdateProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.ListProducts
{
    public interface IListProducts
    {
        public Task<ListProductsOutPut> Handle(ListProductInPut request);
    }
}
