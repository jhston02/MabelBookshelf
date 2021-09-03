using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class AggregateRoot<T> : Entity<T>
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

        protected void AddEvent(DomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        public void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}