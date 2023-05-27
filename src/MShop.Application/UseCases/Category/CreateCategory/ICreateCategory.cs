using MShop.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory
    {
        Task<CategoryModelOutPut> Handler(CreateCategoryInPut request, CancellationToken cancellation);
    }
}
