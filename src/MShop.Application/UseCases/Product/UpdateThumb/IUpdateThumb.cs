using MediatR;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateThumb
{
    public interface IUpdateThumb : IRequestHandler<UpdateThumbInPut, Result<ProductModelOutPut>>
    {
        public Task<Result<ProductModelOutPut>> Handle(UpdateThumbInPut request, CancellationToken cancellationToken);
    }
}
