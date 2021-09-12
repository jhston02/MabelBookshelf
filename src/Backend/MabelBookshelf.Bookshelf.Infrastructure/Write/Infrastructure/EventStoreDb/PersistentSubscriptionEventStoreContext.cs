using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb
{
    //TODO redo this like a human would look at catchup version it's much better than this mess
    public class PersistentSubscriptionEventStoreContext : IDisposable
    {
        private readonly ITypeCache _cache;
        private readonly EventStorePersistentSubscriptionsClient _client;
        private readonly ConcurrentDictionary<string, (StreamStatus, PersistentSubscription)> _subCache;

        public PersistentSubscriptionEventStoreContext(EventStorePersistentSubscriptionsClient client, ITypeCache cache)
        {
            _client = client;
            _cache = cache;
            _subCache = new ConcurrentDictionary<string, (StreamStatus, PersistentSubscription)>();
        }

        public void Dispose()
        {
            foreach (var sub in _subCache) sub.Value.Item2.Dispose();
            _client?.Dispose();
        }

        public async Task<string> Subscribe(string groupName, string streamName, string subscriptionId,
            Func<DomainEvent, Task> action, CancellationToken token)
        {
            var subscription = await _client.SubscribeAsync(
                streamName,
                groupName,
                async (_, e, _, _) =>
                {
                    var linked = e.Link;
                    if (linked == null)
                        throw new Exception("Please link event");
                    var type = _cache.GetTypeFromString(e.Event.EventType);
                    var data = Encoding.UTF8.GetString(e.Event.Data.Span);
                    var serializedData = JsonSerializer.Deserialize(data, type);
                    if (serializedData is DomainEvent castedData)
                        await action(castedData);
                    else
                        throw new Exception("Invalid event type");
                }, (subscription, dropReason, _) =>
                {
                    if (dropReason != SubscriptionDroppedReason.Disposed)
                    {
                        // Resubscribe if the client didn't stop the subscription
                        subscription.Dispose();
                        var valueTuple = _subCache[subscriptionId];
                        _subCache[subscriptionId] = (new StreamStatus(
                                valueTuple.Item1.Id,
                                valueTuple.Item1.Group,
                                valueTuple.Item1.StreamName,
                                Status.Dropped),
                            valueTuple.Item2);
                    }
                }, cancellationToken: token);
            var value = (new StreamStatus(subscriptionId, groupName, streamName, Status.Ok), subscription);
            _subCache.AddOrUpdate(subscriptionId, _ => value, (_, _) => value);
            return subscription.SubscriptionId;
        }

        public StreamStatus GetStreamStatus(string subscriptionId)
        {
            if (_subCache.ContainsKey(subscriptionId))
                return _subCache[subscriptionId].Item1;
            return null;
        }

        public IEnumerable<StreamStatus> GetDroppedStreams()
        {
            return _subCache.Values.Where(x => x.Item1.Status == Status.Dropped).Select(x => x.Item1);
        }
    }
}