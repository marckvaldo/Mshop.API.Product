using MShop.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory
    {
        Task<CategoryModelOutPut> Handler(UpdateCategoryInPut request);
    }
}
