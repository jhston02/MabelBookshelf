﻿namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class NotFinishDomainEvent : BookDomainEvent
    {
        public NotFinishDomainEvent(string bookId, long streamPosition) : base(bookId, streamPosition)
        {
        }
    }
}