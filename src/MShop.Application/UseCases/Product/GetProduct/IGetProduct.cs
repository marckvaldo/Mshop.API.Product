using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public interface IGetProduct : IRequestHandler<GetProductInPut, Result<GetProductOutPut>>
    {
        public Task<Result<GetProductOutPut>> Handle(GetProductInPut request, CancellationToken cancellation);
    }
}
