using MShop.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.GetCatetory
{
    public interface IGetCategory
    {
        Task<CategoryModelOutPut> Handler(Guid id);
    }
}
