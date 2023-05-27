using MShop.Business.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Event
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        Task HandlerAsync(TDomainEvent domainEvent);
    }
}
