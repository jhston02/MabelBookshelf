using System;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class Entity<T>
    {
        private readonly Action<DomainEvent> whenAction;

        protected Entity(T id, Action<DomainEvent> whenAction)
        {
            Id = id;
            this.whenAction = whenAction;
        }

        public T Id { get; protected set; }

        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Entity<T> other)
        {
            if (Id != null && Id.Equals(other.Id))
                return true;
            return false;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity<T>)obj);
        }

        public override int GetHashCode()
        {
            if (Id != null) return Id.GetHashCode();
            return 0;
        }

        protected void When(DomainEvent @event)
        {
            Apply(@event);
            whenAction(@event);
        }

        public abstract void Apply(DomainEvent @event);
    }
}