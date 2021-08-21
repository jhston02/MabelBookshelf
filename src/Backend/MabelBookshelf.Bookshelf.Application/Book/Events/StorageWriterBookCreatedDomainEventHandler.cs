using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Events
{
    public class StorageWriterBookCreatedDomainEventHandler : INotificationHandler<BookCreatedDomainEvent>
    {
        private IEnumerable<IDomainEventWriter<BookCreatedDomainEvent, Guid>> writers;
        public StorageWriterBookCreatedDomainEventHandler(IEnumerable<IDomainEventWriter<BookCreatedDomainEvent, Guid>> writers)
        {
            this.writers = writers;
        }
        
        public async Task Handle(BookCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            foreach (var writer in writers)
            {
                await writer.Handle(notification);
            }
        }
    }
}