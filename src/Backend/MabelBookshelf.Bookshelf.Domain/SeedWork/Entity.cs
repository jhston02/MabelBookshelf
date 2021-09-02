using System;
using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class Entity
    {
        public string Id { get; protected set; }
        public long Version { get; protected set; }
        public bool IsDeleted { get; protected set; }
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Entity other)
        {
            if (Id.Equals(other.Id))
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
        
        protected void AddEvent(DomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        public void ClearEvents()
        {
            _domainEvents.Clear();
        }

        public virtual void Apply(DomainEvent @event) { }

        public abstract void Delete();
    }
}