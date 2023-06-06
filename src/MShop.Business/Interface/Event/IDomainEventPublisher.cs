using MShop.Business.SeedWork;

namespace MShop.Business.Interface.Event
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TDomainEvent>(TDomainEvent entity) where TDomainEvent : DomainEvent;
    }
}
