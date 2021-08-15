using System.Collections.Generic;
using System.Threading.Tasks;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}