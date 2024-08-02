using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Category.GetCategory;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory : IRequestHandler<GetCategoryInPut, Result<CategoryModelOutPut>>
    {
        Task<Result<CategoryModelOutPut>> Handle(GetCategoryInPut id, CancellationToken cancellationToken);
    }
}
