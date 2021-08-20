using System;

namespace MabelBookshelf.Bookshelf.Infrastructure.Interfaces
{
    public interface ITypeCache
    {
        Type GetTypeFromString(string name);
    }
}