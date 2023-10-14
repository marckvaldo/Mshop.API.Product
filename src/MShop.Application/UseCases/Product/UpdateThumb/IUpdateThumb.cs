using MediatR;
using MShop.Application.UseCases.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateThumb
{
    public interface IUpdateThumb : IRequestHandler<UpdateThumbInPut, ProductModelOutPut>
    {
        public Task<ProductModelOutPut> Handle(UpdateThumbInPut request, CancellationToken cancellationToken);
    }
}
