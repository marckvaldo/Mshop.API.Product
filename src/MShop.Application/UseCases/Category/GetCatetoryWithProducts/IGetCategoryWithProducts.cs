using MShop.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory
{
    public interface IGetCategoryWithProducts
    {
        Task<GetCategoryWithProductsOutPut> Handler(Guid id);
    }
}
