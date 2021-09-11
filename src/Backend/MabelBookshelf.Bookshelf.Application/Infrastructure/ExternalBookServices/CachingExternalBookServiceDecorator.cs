using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Application.Models;
using Microsoft.Extensions.Caching.Memory;

namespace MabelBookshelf.Bookshelf.Application.Infrastructure.ExternalBookServices
{
    //TODO: Global cache like redis. This isn't really gonna scale out but it's fine for now
    public class CachingExternalBookServiceDecorator : IExternalBookService
    {
        private readonly IMemoryCache _cache;
        private readonly IExternalBookService _inner;

        public CachingExternalBookServiceDecorator(IMemoryCache cache, IExternalBookService inner)
        {
            _cache = cache;
            _inner = inner;
        }

        public async Task<ExternalBook> GetBookAsync(string externalBookId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            return await _cache.GetOrCreateAsync(externalBookId, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                return await _inner.GetBookAsync(externalBookId, token);
            });
        }
    }
}