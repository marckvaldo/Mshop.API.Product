using MediatR;
using MediatR.Pipeline;
using MShop.Application.Common;
using MShop.Application.UseCases.Category.GetCatetoryWithProducts;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory
{
    public interface IGetCategoryWithProducts : IRequestHandler<GetCategoryWithProductsInPut, Result<GetCategoryWithProductsOutPut>>
    {
        Task<Result<GetCategoryWithProductsOutPut>> Handle(GetCategoryWithProductsInPut request, CancellationToken cancellationToken);
    }
}
