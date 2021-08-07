using System.Collections.Generic;
using MediatR;

namespace MockBookStore.Catalog.Domain.SeedWork
{
    public class Entity
    {
        public long Id { get; private set; }
        protected bool Equals(Entity other)
        {
            if (Id == other.Id)
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
            if (Id != default(long))
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        private List<INotification> domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => domainEvents;
        
        protected void AddEvent(INotification @event)
        {
            domainEvents.Add(@event);
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }
    }
}