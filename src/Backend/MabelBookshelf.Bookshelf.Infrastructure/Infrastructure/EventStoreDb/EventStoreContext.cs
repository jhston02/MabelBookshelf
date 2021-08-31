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
        private readonly ITypeCache _cache;
        
        public EventStoreContext(EventStoreClient client, ITypeCache cache)
        {
            this._client = client;
            this._cache = cache;
        }

        public async Task<T> CreateStreamAsync<T>(T value, string streamName) where T : Entity
        {
            var eventData = GetEventData<T>(value);
            await _client.AppendToStreamAsync(
                streamName,
                StreamState.NoStream,
                eventData
            );

            value.ClearEvents();
            return value;
        }

        public async Task<T> WriteToStreamAsync<T>(T value, string streamName) where T : Entity
        {
            var eventData =  GetEventData<T>(value);
            await _client.AppendToStreamAsync(
                streamName,
                new StreamRevision((ulong)(value.Version - eventData.Count - 1)),
                eventData
            );

            value.ClearEvents();
            return value;
        }

        private  List<EventData> GetEventData<T>(T value) where T : Entity
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

        public async Task<T> ReadFromStreamAsync<T>(string streamName) where T : Entity
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
                if (serializedData is DomainEvent)
                {
                    var castedData = serializedData as DomainEvent;
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
                streamId,
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