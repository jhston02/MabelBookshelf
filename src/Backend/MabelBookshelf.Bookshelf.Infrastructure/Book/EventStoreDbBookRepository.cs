using System;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Book
{
    public class EventStoreDbBookRepository : IBookRepository
    {
        private const string PrependStreamName = "book-";
        private readonly IEventStoreContext _context;
        public EventStoreDbBookRepository(IEventStoreContext context)
        {
            this._context = context;
        }
        
        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();
        public async Task<Domain.Aggregates.BookAggregate.Book> AddAsync(Domain.Aggregates.BookAggregate.Book book)
        {
            try
            {
                return await _context.CreateStreamAsync(book,
                    GetKey(book.Id));
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookDomainException($"Book with id:{book.Id} already exists");
            }
        }

        public async Task<Domain.Aggregates.BookAggregate.Book> GetAsync(Guid bookId)
        {
            return await _context.ReadFromStreamAsync<Domain.Aggregates.BookAggregate.Book>(
                GetKey(bookId));
        }

        private string GetKey(Guid bookId)
        {
            return PrependStreamName + bookId;
        }
    }
}