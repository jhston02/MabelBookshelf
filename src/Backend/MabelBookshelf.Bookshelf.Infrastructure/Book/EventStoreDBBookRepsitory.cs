using System;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;

namespace MabelBookshelf.Bookshelf.Infrastructure.Book
{
    public class EventStoreDBBookRepsitory : IBookRepository
    {
        private const string PrependStreamName = "book-";
        private EventStoreContext _context;
        public EventStoreDBBookRepsitory(EventStoreContext context)
        {
            this._context = context;
        }
        
        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();
        public async Task<Domain.Aggregates.BookAggregate.Book> Add(Domain.Aggregates.BookAggregate.Book book)
        {
            try
            {
                return await _context.WriteToStreamAsync<Domain.Aggregates.BookAggregate.Book, Guid>(book,
                    PrependStreamName + book.Id);
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookDomainException($"Book with id:{book.Id} already exists");
            }
        }

        public Task<Domain.Aggregates.BookAggregate.Book> Get(Guid bookId)
        {
            throw new NotImplementedException();
        }
    }
}