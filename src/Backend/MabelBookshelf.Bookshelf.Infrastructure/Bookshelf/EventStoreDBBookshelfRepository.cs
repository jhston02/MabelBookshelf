using System;
using System.Threading.Tasks;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;

namespace MabelBookshelf.Bookshelf.Infrastructure.Bookshelf
{
    public class EventStoreDbBookshelfRepository : IBookshelfRepository
    {
        private const string PrependStreamName = "bookshelf-";
        private readonly IEventStoreContext _context;
        
        public EventStoreDbBookshelfRepository(IEventStoreContext context)
        {
            this._context = context;
        }

        public IUnitOfWork UnitOfWork => new NoOpUnitOfWork();
        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> AddAsync(Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf)
        {
            try
            {
                return await _context.CreateStreamAsync(bookshelf,
                    GetKey(bookshelf.Id));
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookDomainException($"Bookshelf with id:{bookshelf.Id} already exists");
            }
        }

        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> GetAsync(Guid id, bool includeSoftDeletes = false)
        {
            var bookshelf = await _context.ReadFromStreamAsync<Domain.Aggregates.BookshelfAggregate.Bookshelf>(
                GetKey(id));
            if(!includeSoftDeletes) 
                bookshelf = bookshelf.IsDeleted ? null : bookshelf;
            return bookshelf;
        }

        public async Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> UpdateAsync(Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf)
        {
            try
            {
                return await _context.WriteToStreamAsync(bookshelf, PrependStreamName + bookshelf.Id);
            }
            catch (WrongExpectedVersionException)
            {
                throw new BookshelfDomainException($"Bookshelf was modified multiple times");
            }
        }

        private string GetKey(Guid bookshelfId)
        {
            return PrependStreamName + bookshelfId;
        }
    }
}