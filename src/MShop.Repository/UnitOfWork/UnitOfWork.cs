using Microsoft.Extensions.Logging;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Core.Message.DomainEvent;
using MShop.Repository.Context;

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
                .Entries<Entity>()
                .Where(x => x.Entity.Events.Any())
                .Select(x => x.Entity)
                .ToList();
                
            var events = aggregateRoot
                .SelectMany(a => a.Events)
                .ToList();

            
            /*foreach(var entity in _repositoryDbContext.ChangeTracker.Entries<AggregateRoot>().Where(e => e.Entity.GetType().GetProperty("DataCadastro") != null))
            {

                if (entity.State == EntityState.Added)
                    entity.Property("DataCadastro").CurrentValue = DateTime.Now;

                if (entity.State == EntityState.Modified)
                    entity.Property("DataCadastro").IsModified = false;
            }*/

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
