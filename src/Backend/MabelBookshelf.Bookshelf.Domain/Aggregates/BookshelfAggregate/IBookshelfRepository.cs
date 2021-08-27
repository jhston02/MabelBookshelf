using System;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public interface IBookshelfRepository : IRepository<Bookshelf>
    {
        Task<Bookshelf> Add(Bookshelf bookshelf);
        Task<Bookshelf> Get(Guid id);
        Task<Bookshelf> Update(Guid id);
    }
}