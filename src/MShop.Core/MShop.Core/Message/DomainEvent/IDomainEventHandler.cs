using Core = MShop.Core.DomainObject;

namespace MShop.Core.Message.DomainEvent
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : Core.DomainObject.DomainEvent
    {
        Task HandlerAsync(TDomainEvent domainEvent);
    }
}
