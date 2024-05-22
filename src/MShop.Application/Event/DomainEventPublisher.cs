using Microsoft.Extensions.DependencyInjection;
using MShop.Core.DomainObject;
using MShop.Core.Message.DomainEvent;

namespace MShop.Application.Event
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventPublisher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;


        public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>(); //isso e a mesma coisa que var handles new ProductCreatedEventHandler(); exemplo
            if (handlers is null || !handlers.Any()) return;
            foreach (var handler in handlers)
                await handler.HandlerAsync(domainEvent);
        }        
    }
}
