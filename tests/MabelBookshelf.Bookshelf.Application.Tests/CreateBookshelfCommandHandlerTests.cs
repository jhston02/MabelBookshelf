using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class CreateBookshelfCommandHandlerTests
    {
        [Fact]
        public void CreateBookshelfCommandHandler_ValidCommand_AddsBookshelfToRepository()
        {
            var mockRepo = new MockBookshelfRepository();
            var handler = new CreateBookshelfCommandHandler(mockRepo);
            handler.Handle(new CreateBookshelfCommand(Guid.NewGuid(), "test", 4.ToString()), CancellationToken.None);
            var bookshelf = mockRepo.Bookshelfs.FirstOrDefault(x => x.Name == "test");
            Assert.NotNull(bookshelf);
        }

        private class MockBookshelfRepository : IBookshelfRepository
        {
            public readonly List<Domain.Aggregates.BookshelfAggregate.Bookshelf> Bookshelfs = new();
            public IUnitOfWork UnitOfWork => new MockUnitOfWork();

            public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> AddAsync(
                Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf, CancellationToken token = default)
            {
                Bookshelfs.Add(bookshelf);
                return Task.FromResult(bookshelf);
            }

            public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> GetAsync(Guid id, bool includeSoftDeletes,
                CancellationToken token = default)
            {
                throw new NotImplementedException();
            }

            public Task<Domain.Aggregates.BookshelfAggregate.Bookshelf> UpdateAsync(
                Domain.Aggregates.BookshelfAggregate.Bookshelf bookshelf, CancellationToken token = default)
            {
                throw new NotImplementedException();
            }
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