using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.ListProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.ProductsPromotions
{
    public interface IProductsPromotions
    {
        public Task<List<ProductModelOutPut>> Handler();
    }
}
