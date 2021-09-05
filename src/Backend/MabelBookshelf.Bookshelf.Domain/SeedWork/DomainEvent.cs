using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            Timestamp = DateTimeOffset.UtcNow;
            EventId = Guid.NewGuid();
        }

        public DateTimeOffset Timestamp { get; }
        public long StreamPosition { get; }
        public Guid EventId { get; }
    }
}