using MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Service;
using MShop.Business.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Service
{
    public class ServiceProduct : BaseService, IProductService
    {
        public ServiceProduct(INotification notification) : base(notification)
        {

        }

        public Task Add(Product produtc)
        {
            throw new NotImplementedException();
        }

        public Task Remover(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Product produtc)
        {
            throw new NotImplementedException();
        }

        /*public async Task Add(Product produtc)
        {
            if(!Validate(new ProductValidador(produtc)))
            {
                return;
            }

            Notificar("");
            return;
        }

        public Task Remover(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Product produtc)
        {
            throw new NotImplementedException();
        }*/
    }
}
