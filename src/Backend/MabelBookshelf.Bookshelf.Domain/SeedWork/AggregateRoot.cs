using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class AggregateRoot<T>
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public int Version { get; protected set; } = -1;
        public bool IsDeleted { get; protected set; }
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

        public T Id { get; protected set; }

        private void AddEvent(DomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        protected int GetNextVersion()
        {
            return Version + 1;
        }

        public void ClearEvents()
        {
            _domainEvents.Clear();
        }

        public abstract void Delete();

        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Entity<T> other)
        {
            if (Id.Equals(other.Id))
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity<T>)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected void When(DomainEvent @event)
        {
            AddEvent(@event);
            Apply(@event);
        }

        public abstract void Apply(DomainEvent @event);
    }
}