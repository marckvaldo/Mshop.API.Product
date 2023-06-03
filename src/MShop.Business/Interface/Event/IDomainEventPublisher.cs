using MShop.Business.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Event
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TDomainEvent>(TDomainEvent entity) where TDomainEvent : DomainEvent;  
    }
}
