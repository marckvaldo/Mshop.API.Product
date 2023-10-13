using MediatR;
using MShop.Application.Common;
using MShop.Application.UseCases.Category.Common;
using MShop.Business.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinesEntity = MShop.Business.Entity;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public interface IListCategory : IRequestHandler<ListCategoryInPut, ListCategoryOutPut>
    {
        Task<ListCategoryOutPut> Handle(ListCategoryInPut request, CancellationToken cancellation);
    }
}
