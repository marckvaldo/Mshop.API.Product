using MShop.Application.Common;
using MShop.Application.UseCases.Category.Common;
using MShop.Business.Entity;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public class ListCategoryOutPut : PaginatedListOutPut<CategoryModelOutPut>, IModelOutPut
    {
        public ListCategoryOutPut(
            int page, 
            int perPage, 
            int total, 
            IReadOnlyList<CategoryModelOutPut> itens) 
            : base(page, perPage, total, itens)
        {

        }
    }
}
