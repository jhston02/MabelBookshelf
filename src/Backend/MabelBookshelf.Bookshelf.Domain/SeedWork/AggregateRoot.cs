using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class AggregateRoot<T> : Entity<T>
    {
        public int Version { get; protected set; } = -1;
        public bool IsDeleted { get; protected set; }
        
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

        protected void AddEvent(DomainEvent @event)
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
    }
}