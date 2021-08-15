﻿using System.Collections.Generic;
using MediatR;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class Entity<T>
    {
        public T Id { get; protected set; }
        public long Version { get; protected set; }
        protected bool Equals(Entity<T> other)
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
            return Equals((Entity<T>) obj);
        }

        public override int GetHashCode()
        {
            if (!Id.Equals(default(T)))
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        private List<DomainEvent<T>> domainEvents = new List<DomainEvent<T>>();
        public IReadOnlyCollection<DomainEvent<T>> DomainEvents => domainEvents;
        
        protected void AddEvent(DomainEvent<T> @event)
        {
            domainEvents.Add(@event);
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }

        public virtual void Apply(DomainEvent<T> @event) { }
    }
}