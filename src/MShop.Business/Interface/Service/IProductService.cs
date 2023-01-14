using MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Service
{
    public interface IProductService
    {
        Task Add(Product produtc);

        Task Update(Product produtc);

        Task Remover(Guid id);
    }
}
