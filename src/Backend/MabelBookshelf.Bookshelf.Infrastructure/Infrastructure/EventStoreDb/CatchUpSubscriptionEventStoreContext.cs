using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Application.Models;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class CatchUpSubscriptionEventStoreContext
    {
        private readonly EventStoreClient _client;
        private readonly ITypeCache _cache;
        private readonly ConcurrentDictionary<Guid, StreamSubscription> _subscriptionsByName;

        public CatchUpSubscriptionEventStoreContext(EventStoreClient client,  ITypeCache cache)
        {
            this._client = client;
            this._cache = cache;
            _subscriptionsByName = new ConcurrentDictionary<Guid, StreamSubscription>();
        }

        public async Task<Action> Subscribe(Func<StreamEntry, Task> handleEvent, Func<Task<ProjectionPosition>> getPosition, Func<ProjectionPosition, Task> checkpoint, uint checkpointInterval = 32)
        {
            var streamId = Guid.NewGuid();
            await SubscribeImpl(streamId, handleEvent, getPosition, checkpoint, checkpointInterval);
            return () =>
            {
                _subscriptionsByName.TryRemove(streamId, out StreamSubscription sub);
                sub?.Dispose();
            };
        }

        private async Task SubscribeImpl(Guid streamId, Func<StreamEntry, Task> handleEvent, Func<Task<ProjectionPosition>> getPosition, Func<ProjectionPosition, Task> checkpoint, uint checkpointInterval)
        {
            var position = await getPosition();
            var eventStorePosition = new Position(position.CommitPosition, position.PreparePosition);
            var subscription = await _client.SubscribeToAllAsync(eventStorePosition,
                async (subscription, evnt, cancellationToken) =>
                {
                    var type = _cache.GetTypeFromString(evnt.Event.EventType);
                    var data = Encoding.UTF8.GetString(evnt.Event.Data.Span);
                    var serializedData = JsonSerializer.Deserialize(data, type);
                    if (serializedData is DomainEvent castedData)
                    {
                        var entry = new StreamEntry(evnt.Event.Position.CommitPosition,castedData);
                        await handleEvent(entry);
                    }
                },
                subscriptionDropped: ((subscription, reason, exception) =>
                {
                    Console.WriteLine($"Subscription was dropped due to {reason}. {exception}");
                    if (reason != SubscriptionDroppedReason.Disposed)
                    {
                        subscription.Dispose();
                        Resubscribe(streamId,handleEvent, getPosition, checkpoint, checkpointInterval);
                    }
                }),
                filterOptions: new SubscriptionFilterOptions(
                    EventTypeFilter.ExcludeSystemEvents(),
                    checkpointInterval,
                    checkpointReached:async (subscriptionDroppedReason, streamPosition, token) =>
                    {
                        await checkpoint(new ProjectionPosition(streamPosition.CommitPosition, streamPosition.PreparePosition));
                    })
            );
            _subscriptionsByName.AddOrUpdate(streamId, (x) => subscription, (x, y) => subscription);
        }

        private void Resubscribe(Guid streamId, Func<StreamEntry, Task> handleEvent, Func<Task<ProjectionPosition>> getPosition, Func<ProjectionPosition, Task> checkpoint, uint checkpointInterval)
        {
            Task.Run(() => SubscribeImpl(streamId, handleEvent, getPosition, checkpoint, checkpointInterval)).Wait();
        }
    }
}