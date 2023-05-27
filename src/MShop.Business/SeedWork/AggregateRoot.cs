using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.SeedWork
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<DomainEvent> _events = new();

        protected AggregateRoot():base()
        {}

        public IReadOnlyCollection<DomainEvent> Events 
            => new ReadOnlyCollection<DomainEvent>(_events);

        public void RegisterEvent(DomainEvent @event) 
            => _events.Add(@event);

        public void ClearEvents()
            => _events.Clear();
    }
}
