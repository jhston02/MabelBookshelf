using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class CachingEventStoreContextDecorator : IEventStoreContext
    {
        private readonly ConcurrentDictionary<string, Entity> _cache;
        //Probably inefficient to store the existence here but don't want to deal with marker values
        //etc.
        private readonly HashSet<string> _existenceCache;
        private readonly IEventStoreContext _context;
        public CachingEventStoreContextDecorator(IEventStoreContext context)
        {
            this._context = context;
            this._cache = new ConcurrentDictionary<string, Entity>();
            _existenceCache = new HashSet<string>();
        }
        
        public async Task<T> CreateStreamAsync<T>(T value, string streamName) where T : Entity
        {
            var result = await this._context.CreateStreamAsync(value, streamName);
            _cache.TryAdd(streamName, result);
            return result;
        }

        public async Task<T> WriteToStreamAsync<T>(T value, string streamName) where T : Entity
        {
            var result = await this._context.WriteToStreamAsync(value, streamName);
            _cache.AddOrUpdate(streamName, (_) => result, (_,_) => result);
            return result;
        }

        public async Task<T> ReadFromStreamAsync<T>(string streamName) where T : Entity
        {
            if (_cache.ContainsKey(streamName))
            {
                var entity = _cache[streamName];
                return (T)entity;
            }
            else
            {
                var result = await _context.ReadFromStreamAsync<T>(streamName);
                _cache.TryAdd(streamName, result);
                return result;
            }
        }

        public async Task<bool> StreamExists(string streamId)
        {
            if (_existenceCache.Contains(streamId) || _cache.ContainsKey(streamId))
                return true;
            else
            {
                var result = await _context.StreamExists(streamId);
                if (result)
                    _existenceCache.Add(streamId);
                return result;
            }
        }
    }
}