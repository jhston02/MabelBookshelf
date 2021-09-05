using System.Threading;
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
            _context = context;
        }

        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();

        public async Task<Domain.Aggregates.BookAggregate.Book> AddAsync(Domain.Aggregates.BookAggregate.Book book, CancellationToken token = default)
        {
            try
            {
                return await _context.CreateStreamAsync<Domain.Aggregates.BookAggregate.Book, string>(book,
                    GetKey(book.Id), token);
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookDomainException($"Book with id:{book.Id} already exists");
            }
        }

        public async Task<Domain.Aggregates.BookAggregate.Book> GetAsync(string bookId, CancellationToken token = default)
        {
            return await _context.ReadFromStreamAsync<Domain.Aggregates.BookAggregate.Book, string>(
                GetKey(bookId), token);
        }

        private string GetKey(string bookId)
        {
            return PrependStreamName + bookId;
        }
    }
}