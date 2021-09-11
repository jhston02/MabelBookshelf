using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public interface IBookshelfRepository : IRepository<Bookshelf>
    {
        Task<Bookshelf> AddAsync(Bookshelf bookshelf, CancellationToken token = default);
        Task<Bookshelf> GetAsync(Guid id, bool includeSoftDeletes = false, CancellationToken token = default);
        Task<Bookshelf> UpdateAsync(Bookshelf bookshelf, CancellationToken token = default);
    }
}