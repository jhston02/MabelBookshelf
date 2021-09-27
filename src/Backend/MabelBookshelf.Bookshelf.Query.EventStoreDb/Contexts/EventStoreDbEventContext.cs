using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Query.Interfaces;

namespace MabelBookshelf.Bookshelf.Query.EventStoreDb
{
    public class EventStoreDbEventContext : IEventContext
    {
        private readonly EventStoreClient _client;

        public EventStoreDbEventContext(EventStoreClient client)
        {
            _client = client;
        }


        public async Task AppendEvent(string streamName, CancellationToken token = default, params DomainEvent[] events)
        {
            if (events == null || !events.Any()) return;
            
            var eventData = events
                .Select(@event =>
                    new EventData(
                        eventId: Uuid.FromGuid(@event.EventId),
                        type: @event.GetType().Name,
                        data: JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType()
                    )))
                .ToArray();
            
            await _client.AppendToStreamAsync(
                streamName,
                StreamState.Any, 
                eventData, cancellationToken: token);
        }
    }
}