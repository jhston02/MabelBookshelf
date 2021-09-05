using System;
using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class DictionaryTypeCache : ITypeCache
    {
        private readonly Dictionary<string, Type> cache;

        public DictionaryTypeCache(Dictionary<string, Type> cache)
        {
            this.cache = cache;
        }

        public Type GetTypeFromString(string name)
        {
            if (cache.ContainsKey(name))
                return cache[name];
            return null;
        }
    }
}