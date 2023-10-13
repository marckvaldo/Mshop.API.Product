using MediatR;
using MShop.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.GetCategory
{
    public class GetCategoryInPut :IRequest<CategoryModelOutPut>
    {
        public GetCategoryInPut(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
