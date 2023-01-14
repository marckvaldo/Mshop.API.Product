using MShop.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProduct : IDeleteProduct
    {
        public Task<ProductModelOutPut> Handle(Guid request)
        {
            throw new NotImplementedException();
        }
    }
}
