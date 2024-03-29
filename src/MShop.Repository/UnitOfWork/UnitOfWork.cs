﻿using Microsoft.Extensions.Logging;
using MShop.Business.Interface.Event;
using MShop.Business.Interface.Repository;
using MShop.Business.SeedWork;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryDbContext _repositoryDbContext;
        private readonly IDomainEventPublisher _publisher;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(RepositoryDbContext repositoryDbContext, 
            IDomainEventPublisher publisher, 
            ILogger<UnitOfWork> logger)
        {
            _repositoryDbContext = repositoryDbContext;
            _publisher = publisher;
            _logger = logger;   
            
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            
            var aggregateRoot = _repositoryDbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.Events.Any())
                .Select(x => x.Entity)
                .ToList();
                
            var events = aggregateRoot
                .SelectMany(a => a.Events)
                .ToList();

            await _repositoryDbContext.SaveChangesAsync(cancellationToken);

            foreach (var @event in events)
                await _publisher.PublishAsync((dynamic)@event);

            aggregateRoot.ForEach(x=> x.ClearEvents());

        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
