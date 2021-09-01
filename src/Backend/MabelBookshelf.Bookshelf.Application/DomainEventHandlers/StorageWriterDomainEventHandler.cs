using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.DomainEventHandlers
{
    public class StorageWriterDomainEventHandler<T> : INotificationHandler<DomainEvent>
    {
        private IDomainEventWriter writer;
        public StorageWriterDomainEventHandler(IDomainEventWriter writer)
        {
            this.writer = writer;
        }
        
        public async Task Handle(DomainEvent notification, CancellationToken cancellationToken)
        {
            await writer.Write(notification);
        }
    }
}