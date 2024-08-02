using MShop.Core.Message;
using System.Collections.ObjectModel;

namespace MShop.Core.DomainObject
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        private readonly List<DomainEvent> _events = new();

        public IReadOnlyCollection<DomainEvent> Events
            => new ReadOnlyCollection<DomainEvent>(_events);

        public void RegisterEvent(DomainEvent @event)
            => _events?.Add(@event);

        public void ClearEvents()
            => _events?.Clear();

        public virtual bool IsValid(INotification notification)
        {
            throw new NotImplementedException();
        }

        /*public virtual bool IsValid2(INotification notification)
        {
            throw new NotImplementedException();
        }*/








        public override bool Equals(object obj)
        {
            var compareto = obj as Entity;

            if(ReferenceEquals(this, compareto)) return true;
            if(ReferenceEquals(null, compareto)) return false;

            return Id.Equals(compareto.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if(ReferenceEquals(a,null) && ReferenceEquals(b,null)) return true;
            if(ReferenceEquals(a,null) || ReferenceEquals(b,null)) return false; 

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

    }
}

