#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Tests.Mocks
{
    internal class MockBookshelfRepository : IBookshelfRepository
    {
        public readonly List<Domain.Aggregates.BookshelfAggregate.Bookshelf> Bookshelves;

        public MockBookshelfRepository(List<Domain.Aggregates.BookshelfAggregate.Bookshelf> bookshelves)
        {
            Bookshelves = bookshelves;
        }

        public IUnitOfWork UnitOfWork => new MockUnitOfWork();

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> AddAsync(
            Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf, CancellationToken token = default)
        {
            Bookshelves.Add(bookshelf);
            return Task.FromResult(bookshelf);
        }

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf?> GetAsync(Guid id, bool includeSoftDeletes,
            CancellationToken token = default)
        {
            return Task.FromResult(Bookshelves.FirstOrDefault(x => x.Id == id));
        }

        public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf?> UpdateAsync(
            Domain.Aggregates.BookshelfAggregate.Bookshelf? bookshelf, CancellationToken token = default)
        {
            var oldBookshelf = Bookshelves.FirstOrDefault(x => bookshelf != null && x.Id == bookshelf.Id);
            if (oldBookshelf != null) Bookshelves.Remove(oldBookshelf);
            if (bookshelf != null) Bookshelves.Add(bookshelf);
            return Task.FromResult(bookshelf);
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