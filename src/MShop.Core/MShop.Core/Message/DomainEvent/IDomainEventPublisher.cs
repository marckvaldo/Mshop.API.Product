using Core = MShop.Core.DomainObject;

namespace MShop.Core.Message.DomainEvent
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TDomainEvent>(TDomainEvent entity) where TDomainEvent : Core.DomainObject.DomainEvent;
    }
}
