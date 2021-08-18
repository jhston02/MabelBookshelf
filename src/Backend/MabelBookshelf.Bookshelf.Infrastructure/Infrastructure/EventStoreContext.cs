using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MediatR;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class EventStoreContext
    {
        private readonly EventStoreClient _client;
        private readonly IMediator _mediator;
        
        public EventStoreContext(EventStoreClient client, IMediator mediator)
        {
            this._client = client;
            this._mediator = mediator;
        }

        public async Task<T> WriteToStreamAsync<T, V>(T value, string streamName) where T : Entity<V>
        {
            var eventData = new List<EventData>();
            foreach (var @event in value.DomainEvents)
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
                streamName,
                StreamState.NoStream,
                eventData
            );

            value.ClearEvents();
            return value;
        }

        public async Task<bool> StreamExists(string streamId)
        {
            var result = _client.ReadStreamAsync(
                Direction.Forwards,
                "streamId",
                revision: StreamPosition.Start,
                maxCount: 1);

            if (await result.ReadState == ReadState.StreamNotFound)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}