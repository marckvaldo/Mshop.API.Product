using MediatR;
using MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.GetCatetoryWithProducts
{
    public class GetCategoryWithProductsInPut : IRequest<GetCategoryWithProductsOutPut>
    {
        public GetCategoryWithProductsInPut(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
