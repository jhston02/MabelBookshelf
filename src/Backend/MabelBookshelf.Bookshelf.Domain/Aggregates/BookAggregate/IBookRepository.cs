using System;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> AddAsync(Book book);
        Task<Book> GetAsync(string bookId);
        Task<Book> UpdateAsync(Book book);
        Task<bool> Exists(string bookId);
        
    }
}