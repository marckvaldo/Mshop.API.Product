using MediatR;
using MShop.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInPut, CategoryModelOutPut>
    {
        Task<CategoryModelOutPut> Handle(CreateCategoryInPut request, CancellationToken cancellation);
    }
}
