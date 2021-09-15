#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class CreateBookshelfCommandHandlerTests
    {
        [Fact]
        public async Task CreateBookshelfCommandHandler_ValidCommand_AddsBookshelfToRepository()
        {
            var mockRepo = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>());
            var handler = new CreateBookshelfCommandHandler(mockRepo);
            await handler.Handle(new CreateBookshelfCommand(Guid.NewGuid(), "test", 4.ToString()), CancellationToken.None);
            var bookshelf = mockRepo.Bookshelves.FirstOrDefault(x => x.Name == "test");
            Assert.NotNull(bookshelf);
        }

        [Theory]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public async Task CreateBookshelfCommandHandler_InvalidCommand_ThrowsArgumentException(string? name, string? ownerId)
        {
            var mockRepo = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>());
            var handler = new CreateBookshelfCommandHandler(mockRepo);
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await handler.Handle(new CreateBookshelfCommand(Guid.NewGuid(), name, ownerId),
                    CancellationToken.None));
        }

    }
}