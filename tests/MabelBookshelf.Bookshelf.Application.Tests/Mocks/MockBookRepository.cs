#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Tests.Mocks
{
    public class MockBookRepository : IBookRepository
    {
        public List<Domain.Aggregates.BookAggregate.Book> Books;
        public IUnitOfWork UnitOfWork => new MockUnitOfWork();

        public MockBookRepository(List<Domain.Aggregates.BookAggregate.Book> books)
        {
            this.Books = books;
        }
        
        public Task<Domain.Aggregates.BookAggregate.Book> AddAsync(Domain.Aggregates.BookAggregate.Book book, CancellationToken token = default)
        {
            Books.Add(book);
            return Task.FromResult(book);
        }

        public Task<Domain.Aggregates.BookAggregate.Book?> GetAsync(string bookId, CancellationToken token = default)
        {
            return Task.FromResult(Books.FirstOrDefault(x => x.Id == bookId));
        }

        public Task<bool> ExistsAsync(string bookId, CancellationToken token = default)
        {
            return Task.FromResult(Books.Any(x => x.Id == bookId));
        }

        public Task<Domain.Aggregates.BookAggregate.Book?> UpdateAsync(Domain.Aggregates.BookAggregate.Book book, CancellationToken token = default)
        {
            var oldBook = Books.FirstOrDefault(x => book != null && x.Id == book.Id);
            if (oldBook != null) Books.Remove(oldBook);
            if (book != null) Books.Add(book);
            return Task.FromResult(book);
        }
        
        private class MockUnitOfWork : IUnitOfWork
        {
            public Task SaveChangesAsync()
            {
                return Task.CompletedTask;
            }
        }
    }
}