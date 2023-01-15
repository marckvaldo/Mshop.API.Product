﻿using MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MShop.Business.Validator
{
    public class ProductValidador : Notification
    {
        private readonly Product _product;

        public ProductValidador(Product product, INotification notification):base(notification)
        {
            _product = product;
        }

        public override INotification Validate()
        {

            ValidationDefault.NotNullOrEmpty(_product.Description, nameof(_product.Description), _notifications);
            ValidationDefault.MinLength(_product.Description, 10, nameof(_product.Description), _notifications);
            ValidationDefault.MaxLength(_product.Description, 1000, nameof(_product.Description), _notifications);

            //Name
            ValidationDefault.NotNullOrEmpty(_product.Name, nameof(_product.Name), _notifications);
            ValidationDefault.MinLength(_product.Name, 3, nameof(_product.Name), _notifications);
            ValidationDefault.MaxLength(_product.Name, 255, nameof(_product.Name), _notifications);

            //Price
            ValidationDefault.IsPositiveNumber(_product.Price, nameof(_product.Price), _notifications);
            ValidationDefault.IsBiggerOrEqualThan(_product.Price,0, nameof(_product.Price), _notifications);

            //stok
            ValidationDefault.IsLessThan(_product.Stock, 0, nameof(_product.Stock), _notifications);    

            return _notifications;

        }
            

    }
}
