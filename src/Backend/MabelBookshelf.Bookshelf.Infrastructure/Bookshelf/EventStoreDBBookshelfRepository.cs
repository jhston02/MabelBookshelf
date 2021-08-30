using System;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;

namespace MabelBookshelf.Bookshelf.Infrastructure.Bookshelf
{
    public class EventStoreDbBookshelfRepository : IBookshelfRepository
    {
        private const string PrependStreamName = "bookshelf-";
        private readonly EventStoreContext _context;
        
        public EventStoreDbBookshelfRepository(EventStoreContext context)
        {
            this._context = context;
        }

        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();
        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> Add(Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf)
        {
            try
            {
                return await _context.CreateStreamAsync(bookshelf,
                    PrependStreamName + bookshelf.Id);
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookDomainException($"Bookshelf with id:{bookshelf.Id} already exists");
            }
        }

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> Update(Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf)
        {
            throw new NotImplementedException();
        }
    }
}