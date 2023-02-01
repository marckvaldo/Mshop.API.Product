using MShop.Application.UseCases.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public interface IDeleteProduct
    {
        public Task<ProductModelOutPut> Handle(Guid request);
    }
}
