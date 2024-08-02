using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public class GetProductInPut : IRequest<Result<GetProductOutPut>>
    {
        public GetProductInPut(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
