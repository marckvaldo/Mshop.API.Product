using MShop.Application.Common;
using MShop.Application.UseCases.Product.CreateProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public interface IGetProduct
    {
        public Task<ProductModelOutPut> Handle(Guid Id);
    }
}
