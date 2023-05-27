﻿using Microsoft.Extensions.DependencyInjection;
using MShop.Business.Interface.Event;
using MShop.Business.SeedWork;

namespace MShop.Application.Event
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventPublisher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;


        public async Task PublishAsync(DomainEvent domainEvent)
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<DomainEvent>>();
            if (handlers is null || !handlers.Any()) return;
            foreach (var handler in handlers)
            {
                await handler.HandlerAsync(domainEvent);
            }
        }
    }
}