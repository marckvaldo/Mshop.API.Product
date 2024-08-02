using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.ListProducts
{
    public interface IListProducts : IRequestHandler<ListProductInPut, Result<ListProductsOutPut>>
    {
        public Task<Result<ListProductsOutPut>> Handle(ListProductInPut request, CancellationToken cancellation);
    }
}
