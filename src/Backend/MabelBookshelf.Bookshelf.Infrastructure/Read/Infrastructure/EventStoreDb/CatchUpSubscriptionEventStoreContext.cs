using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Application.Models;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using MabelBookshelf.Bookshelf.Infrastructure.Models;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb
{
    public class CatchUpSubscriptionEventStoreContext
    {
        private readonly ITypeCache _cache;
        private readonly EventStoreClient _client;
        private readonly ConcurrentDictionary<Guid, StreamSubscription> _subscriptionsByName;

        public CatchUpSubscriptionEventStoreContext(EventStoreClient client, ITypeCache cache)
        {
            _client = client;
            _cache = cache;
            _subscriptionsByName = new ConcurrentDictionary<Guid, StreamSubscription>();
        }

        public async Task<Action> Subscribe(Func<StreamEntry, CancellationToken, Task> handleEvent,
            Func<CancellationToken, Task<ProjectionPosition>> getPosition,
            Func<ProjectionPosition, CancellationToken, Task> checkpoint, uint checkpointInterval = 32,
            CancellationToken token = default)
        {
            var streamId = Guid.NewGuid();
            await SubscribeImpl(streamId, handleEvent, getPosition, checkpoint, checkpointInterval, token);
            return () =>
            {
                try
                {
                    _subscriptionsByName.TryRemove(streamId, out var sub);
                    sub?.Dispose();
                }
                catch (Exception)
                {
                    //swallow exception, oh well
                }
            };
        }

        private async Task SubscribeImpl(Guid streamId, Func<StreamEntry, CancellationToken, Task> handleEvent,
            Func<CancellationToken, Task<ProjectionPosition>> getPosition,
            Func<ProjectionPosition, CancellationToken, Task> checkpoint, uint checkpointInterval,
            CancellationToken token)
        {
            var position = await getPosition(token);
            var eventStorePosition = position == null
                ? Position.Start
                : new Position(position.CommitPosition, position.PreparePosition);
            var subscription = await _client.SubscribeToAllAsync(eventStorePosition,
                async (_, evnt, cancellationToken) =>
                {
                    var type = _cache.GetTypeFromString(evnt.Event.EventType);
                    var data = Encoding.UTF8.GetString(evnt.Event.Data.Span);
                    var serializedData = JsonSerializer.Deserialize(data, type);
                    if (serializedData is DomainEvent castedData)
                    {
                        var entry = new StreamEntry(evnt.Event.Position.CommitPosition, castedData);
                        await handleEvent(entry, cancellationToken);
                    }
                },
                subscriptionDropped: (subscription, reason, exception) =>
                {
                    Console.WriteLine($"Subscription was dropped due to {reason}. {exception}");
                    if (reason != SubscriptionDroppedReason.Disposed)
                    {
                        subscription.Dispose();
                        Resubscribe(streamId, handleEvent, getPosition, checkpoint, checkpointInterval, token);
                    }
                },
                filterOptions: new SubscriptionFilterOptions(
                    EventTypeFilter.ExcludeSystemEvents(),
                    checkpointInterval,
                    async (subscriptionDroppedReason, streamPosition, cancellationToken) =>
                    {
                        await checkpoint(
                            new ProjectionPosition(streamPosition.CommitPosition, streamPosition.PreparePosition),
                            cancellationToken);
                    }), cancellationToken: token);
            _subscriptionsByName.AddOrUpdate(streamId, x => subscription, (x, y) => subscription);
        }

        private void Resubscribe(Guid streamId, Func<StreamEntry, CancellationToken, Task> handleEvent,
            Func<CancellationToken, Task<ProjectionPosition>> getPosition,
            Func<ProjectionPosition, CancellationToken, Task> checkpoint, uint checkpointInterval,
            CancellationToken token)
        {
            Task.Run(() => SubscribeImpl(streamId, handleEvent, getPosition, checkpoint, checkpointInterval, token),
                token).Wait(token);
        }
    }
}