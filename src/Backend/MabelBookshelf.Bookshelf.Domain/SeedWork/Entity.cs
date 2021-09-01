using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.VisualBasic.FileIO;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public long Version { get; protected set; }
        public bool IsDeleted { get; protected set; }
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
            if (!Id.Equals(default(Guid)))
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        private List<DomainEvent> domainEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents;
        
        protected void AddEvent(DomainEvent @event)
        {
            domainEvents.Add(@event);
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }

        public virtual void Apply(DomainEvent @event) { }

        public abstract void Delete();
    }
}