using MShop.Business.Entity;
using MShop.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Validator
{
    public class ProductValidador : Notification
    {
        private readonly Product _product;

        public ProductValidador(Product product)
        {
            _product = product;
        }

        public override Notifications Validate()
        {
            if(_product.Description!= null)
            {
                _handler.AddNotifications("Descrição não pode ser vazia!");
            }


            return _handler;
        }

    }
}
