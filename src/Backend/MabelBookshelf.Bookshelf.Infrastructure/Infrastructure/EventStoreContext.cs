using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using MediatR;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class EventStoreContext
    {
        private readonly EventStoreClient _client;
        private readonly IMediator _mediator;
        private readonly ITypeCache _cache;
        
        public EventStoreContext(EventStoreClient client, IMediator mediator, ITypeCache cache)
        {
            this._client = client;
            this._mediator = mediator;
            this._cache = cache;
        }

        public async Task<T> WriteToStreamAsync<T, V>(T value, string streamName) where T : Entity<V>
        {
            var eventData = new List<EventData>();
            foreach (var @event in value.DomainEvents)
            {
                await _mediator.Publish(@event);
                var serializedData = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
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

        public async Task<T> ReadFromStreamAsync<T,V>(string streamName) where T : Entity<V>
        {
            var result = _client.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.Start);

            if (await result.ReadState == ReadState.StreamNotFound) {
                return null;
            }

            var entity = Activator.CreateInstance(typeof(T), true) as T;
            await foreach (var e in result)
            {
                var type = _cache.GetTypeFromString(e.Event.EventType);
                var data = Encoding.UTF8.GetString(e.Event.Data.Span);
                var serializedData = JsonSerializer.Deserialize(data, type);
                if (serializedData is DomainEvent<V>)
                {
                    var castedData = serializedData as DomainEvent<V>;
                    entity.Apply(castedData);
                }
                else
                    throw new Exception("Invalid event type");
            }

            return entity;
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