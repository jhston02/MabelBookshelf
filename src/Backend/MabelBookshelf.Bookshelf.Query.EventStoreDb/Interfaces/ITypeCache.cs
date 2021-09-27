using System;

namespace MabelBookshelf.Bookshelf.Query.EventStoreDb.Interfaces
{
    public interface ITypeCache
    {
        Type GetTypeFromString(string name);
    }
}