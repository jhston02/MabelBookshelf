using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class DeleteBookshelfCommandHandlerTests
    {
        [Fact]
        public async Task DeleteBookshelfCommandHandler_BookshelfExistsIsValid_IsDeleted()
        {
            var id = Guid.NewGuid();
            var repository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>
                { new(id, "test", "test") });
            var command = new DeleteBookshelfCommand(id);
            var commandHandler = new DeleteBookshelfCommandHandler(repository);
            var result = await commandHandler.Handle(command, CancellationToken.None);
            var bookshelf = repository.Bookshelves.FirstOrDefault();
            Assert.True(bookshelf is { IsDeleted: true });
        }

        [Fact]
        public async Task DeleteBookshelfCommandHandler_BookshelfNotExists_ThrowsException()
        {
            var id = Guid.NewGuid();
            var repository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>());
            var command = new DeleteBookshelfCommand(id);
            var commandHandler = new DeleteBookshelfCommandHandler(repository);
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
                await commandHandler.Handle(command, CancellationToken.None));
        }
    }
}