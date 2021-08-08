using System;
using MediatR;

namespace MockBookStore.Bookshelf.Domain.SeedWork
{
    public abstract class DomainEvent<T> : INotification
    {
        protected DomainEvent(T streamId, long streamPosition)
        {
            StreamId = streamId;
            StreamPosition = streamPosition;
            Timestamp = DateTimeOffset.UtcNow;
        }
        public T StreamId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public long StreamPosition { get; private set; }
    }
}