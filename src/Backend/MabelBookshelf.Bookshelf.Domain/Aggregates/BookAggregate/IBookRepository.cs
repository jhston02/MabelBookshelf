using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> AddAsync(Book book, CancellationToken token = default);
        Task<Book> GetAsync(string bookId, CancellationToken token = default);
    }
}