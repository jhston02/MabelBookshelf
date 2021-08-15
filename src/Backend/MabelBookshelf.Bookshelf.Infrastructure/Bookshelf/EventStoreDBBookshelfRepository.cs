using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;
using MediatR;

namespace MabelBookshelf.Bookshelf.Infrastructure.Bookshelf
{
    public class EventStoreDBBookshelfRepository : IBookshelfRepository
    {
        private const string PrependStreamName = "bookshelf-";
        private readonly EventStoreClient _client;
        private readonly IMediator _mediator;
        
        public EventStoreDBBookshelfRepository(EventStoreClient client, IMediator mediator)
        {
            this._client = client;
            this._mediator = mediator;
        }

        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();
        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> Add(Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf)
        {
            var eventData = new List<EventData>();
            foreach (var @event in bookshelf.DomainEvents)
            {
                await _mediator.Publish(@event);
                var serializedData = JsonSerializer.SerializeToUtf8Bytes(@event);
                eventData.Add(
                    new EventData(
                        Uuid.FromGuid(@event.EventId),
                        @event.GetType().Name,
                        serializedData
                    ));
            }

            await _client.AppendToStreamAsync(
                PrependStreamName + bookshelf.Id,
                StreamState.NoStream,
                eventData
            );

            bookshelf.ClearEvents();
            return bookshelf;
        }
    }
}