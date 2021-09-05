using System;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public interface IBookshelfRepository : IRepository<Bookshelf>
    {
        Task<Bookshelf> AddAsync(Bookshelf bookshelf);
        Task<Bookshelf> GetAsync(Guid id, bool includeSoftDeletes = false);
        Task<Bookshelf> UpdateAsync(Bookshelf bookshelf);
    }
}