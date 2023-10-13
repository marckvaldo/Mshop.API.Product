using MediatR;
using MediatR.Pipeline;
using MShop.Application.Common;
using MShop.Application.UseCases.Category.GetCatetoryWithProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory
{
    public interface IGetCategoryWithProducts : IRequestHandler<GetCategoryWithProductsInPut, GetCategoryWithProductsOutPut>
    {
        Task<GetCategoryWithProductsOutPut> Handle(GetCategoryWithProductsInPut request, CancellationToken cancellationToken);
    }
}
