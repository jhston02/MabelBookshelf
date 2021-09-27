using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb
{
    public class EventStoreContext : IEventStoreContext
    {
        private readonly ITypeCache _cache;
        private readonly EventStoreClient _client;

        public EventStoreContext(EventStoreClient client, ITypeCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<T> CreateStreamAsync<T, TV>(T value, string streamName, CancellationToken token = default)
            where T : AggregateRoot<TV>
        {
            var eventData = GetEventData<T, TV>(value);
            await _client.AppendToStreamAsync(
                streamName,
                StreamState.NoStream,
                eventData, cancellationToken: token);

            value.ClearEvents();
            return value;
        }

        public async Task<T> WriteToStreamAsync<T, TV>(T value, string streamName, CancellationToken token = default)
            where T : AggregateRoot<TV>
        {
            var eventData = GetEventData<T, TV>(value);
            await _client.AppendToStreamAsync(
                streamName,
                new StreamRevision((ulong)(value.Version - eventData.Count)),
                eventData, cancellationToken: token);

            value.ClearEvents();
            return value;
        }

        public async Task<T?> ReadFromStreamAsync<T, TV>(string streamName, CancellationToken token = default)
            where T : AggregateRoot<TV>
        {
            var result = _client.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.Start, cancellationToken: token);

            if (await result.ReadState == ReadState.StreamNotFound) return null;

            var entity = Activator.CreateInstance(typeof(T), true) as T;
            await foreach (var e in result)
            {
                var type = _cache.GetTypeFromString(e.Event.EventType);
                var data = Encoding.UTF8.GetString(e.Event.Data.Span);
                var serializedData = JsonSerializer.Deserialize(data, type);
                if (serializedData is DomainEvent castedData)
                {
                    if (entity != null) entity.Apply(castedData);
                }
                else
                {
                    throw new Exception("Invalid event type");
                }
            }

            return entity;
        }

        public async Task<bool> StreamExists(string streamId, CancellationToken token = default)
        {
            var result = _client.ReadStreamAsync(
                Direction.Forwards,
                streamId,
                StreamPosition.Start,
                1, cancellationToken: token);

            if (await result.ReadState == ReadState.StreamNotFound)
                return false;
            return true;
        }

        private List<EventData> GetEventData<T, TV>(T value) where T : AggregateRoot<TV>
        {
            var eventData = new List<EventData>();
            foreach (var @event in value.DomainEvents)
            {
                var serializedData = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
                eventData.Add(
                    new EventData(
                        Uuid.FromGuid(@event.EventId),
                        @event.GetType().Name,
                        serializedData
                    ));
            }

            return eventData;
        }
    }
}