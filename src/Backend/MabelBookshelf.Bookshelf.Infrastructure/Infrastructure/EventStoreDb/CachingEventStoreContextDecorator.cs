using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class CachingEventStoreContextDecorator : IEventStoreContext
    {
        private readonly ConcurrentDictionary<string, object> _cache;

        private readonly IEventStoreContext _context;

        //Probably inefficient to store the existence here but don't want to deal with marker values
        //etc.
        private readonly HashSet<string> _existenceCache;

        public CachingEventStoreContextDecorator(IEventStoreContext context)
        {
            _context = context;
            _cache = new ConcurrentDictionary<string, object>();
            _existenceCache = new HashSet<string>();
        }

        public async Task<T> CreateStreamAsync<T, TV>(T value, string streamName) where T : AggregateRoot<TV>
        {
            var result = await _context.CreateStreamAsync<T, TV>(value, streamName);
            _cache.TryAdd(streamName, result);
            return result;
        }

        public async Task<T> WriteToStreamAsync<T, TV>(T value, string streamName) where T : AggregateRoot<TV>
        {
            var result = await _context.WriteToStreamAsync<T, TV>(value, streamName);
            _cache.AddOrUpdate(streamName, _ => result, (_, _) => result);
            return result;
        }

        public async Task<T> ReadFromStreamAsync<T, TV>(string streamName) where T : AggregateRoot<TV>
        {
            if (_cache.ContainsKey(streamName))
            {
                var entity = _cache[streamName];
                return (T)entity;
            }

            var result = await _context.ReadFromStreamAsync<T, TV>(streamName);
            _cache.TryAdd(streamName, result);
            return result;
        }

        public async Task<bool> StreamExists(string streamId)
        {
            if (_existenceCache.Contains(streamId) || _cache.ContainsKey(streamId)) return true;

            var result = await _context.StreamExists(streamId);
            if (result)
                _existenceCache.Add(streamId);
            return result;
        }
    }
}