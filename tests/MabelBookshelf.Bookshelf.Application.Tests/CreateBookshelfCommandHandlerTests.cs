using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    using Bookshelf=Domain.Aggregates.BookshelfAggregate.Bookshelf;
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
            public List<Bookshelf> Bookshelfs = new List<Bookshelf>();
            public IUnitOfWork UnitOfWork => new MockUnitOfWork();
            public Task<Bookshelf> Add(Bookshelf bookshelf)
            {
                Bookshelfs.Add(bookshelf);
                return Task.FromResult(bookshelf);
            }

            public Task<Bookshelf> Update(Bookshelf bookshelf)
            {
                throw new NotImplementedException();
            }

            public Task<Bookshelf> Get(Guid id)
            {
                throw new NotImplementedException();
            }
        }
        
        private  class MockUnitOfWork : IUnitOfWork
        {
            public Task SaveChangesAsync()
            {
                return Task.CompletedTask;
            }
        }
    }
}