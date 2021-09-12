using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Bookshelf
{
    public class EventStoreDbBookshelfRepository : IBookshelfRepository
    {
        private const string PrependStreamName = "bookshelf-";
        private readonly IEventStoreContext _context;

        public EventStoreDbBookshelfRepository(IEventStoreContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();

        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> AddAsync(
            Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf, CancellationToken token = default)
        {
            try
            {
                return await _context.CreateStreamAsync<Domain.Aggregates.BookshelfAggregate.Bookshelf, Guid>(bookshelf,
                    GetKey(bookshelf.Id), token);
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookDomainException($"Bookshelf with id:{bookshelf.Id} already exists");
            }
        }

        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> GetAsync(Guid id,
            bool includeSoftDeletes = false, CancellationToken token = default)
        {
            var bookshelf = await _context.ReadFromStreamAsync<Domain.Aggregates.BookshelfAggregate.Bookshelf, Guid>(
                GetKey(id), token);
            if (!includeSoftDeletes)
                bookshelf = bookshelf?.IsDeleted ?? true ? null : bookshelf;
            return bookshelf;
        }

        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> UpdateAsync(
            Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf, CancellationToken token = default)
        {
            try
            {
                return await _context.WriteToStreamAsync<Domain.Aggregates.BookshelfAggregate.Bookshelf, Guid>(
                    bookshelf, GetKey(bookshelf.Id), token);
            }
            catch (WrongExpectedVersionException)
            {
                throw new Exception("Bookshelf was modified multiple times");
            }
        }

        private string GetKey(Guid bookshelfId)
        {
            return PrependStreamName + bookshelfId;
        }
    }
}