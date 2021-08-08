using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MockBookStore.Bookshelf.Domain.SeedWork
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T> Get(long id);
        Task<IEnumerable<T>> List();
        T Add(T value);
        T Update(T value);
        void Delete(long id);
    }
}