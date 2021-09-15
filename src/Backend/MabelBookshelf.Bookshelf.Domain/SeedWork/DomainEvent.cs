using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract record DomainEvent : INotification
    {
        protected DomainEvent()
        {
            Timestamp = DateTimeOffset.UtcNow;
            EventId = Guid.NewGuid();
        }

        public DateTimeOffset Timestamp { get; }
        public Guid EventId { get; }
    }
}