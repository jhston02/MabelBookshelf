#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Tests.Mocks
{

    internal class MockBookshelfRepository : IBookshelfRepository
    {
        public readonly List<Domain.Aggregates.BookshelfAggregate.Bookshelf> Bookshelves;
        public IUnitOfWork UnitOfWork => new MockUnitOfWork();

        public MockBookshelfRepository(List<Domain.Aggregates.BookshelfAggregate.Bookshelf> bookshelves)
        {
            Bookshelves = bookshelves;
        }
        
        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> AddAsync(
            Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf, CancellationToken token = default)
        {
            Bookshelves.Add(bookshelf);
            return Task.FromResult(bookshelf);
        }

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf?> GetAsync(Guid id, bool includeSoftDeletes,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf?> UpdateAsync(
            Domain.Aggregates.BookshelfAggregate.Bookshelf? bookshelf, CancellationToken token = default)
        {
            throw new NotImplementedException();
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