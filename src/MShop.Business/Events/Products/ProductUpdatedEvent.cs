using MShop.Business.Entity;
using MShop.Business.SeedWork;
using MShop.Business.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Events.Products
{
    public class ProductUpdatedEvent : DomainEvent
    {
        public ProductUpdatedEvent(Guid productId)
        {
            ProductId = productId;
           
        }

        public Guid ProductId { get; private set; }

    }
}
