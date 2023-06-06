using MShop.Business.SeedWork;

namespace MShop.Business.Interface.Event
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        Task HandlerAsync(TDomainEvent domainEvent);
    }
}
