#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class CreateBookCommandHandlerTests
    {
        [Fact]
        public async Task CreateBookCommandHandler_ValidCommand_AddsBookToRepository()
        {
            var mockRepo = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>());
            var mockExternalBookService = new MockExternalBookService();
            var handler = new CreateBookCommandHandler(mockExternalBookService, mockRepo);
            await handler.Handle(new CreateBookCommand("test", "blah"), CancellationToken.None);
            var book = mockRepo.Books.FirstOrDefault(x => x.VolumeInfo.ExternalId == "test");
            Assert.NotNull(book);
        }

        [Theory]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public async Task CreateBookCommandHandler_InvalidCommand_ThrowsArgumentException(string? externalId,
            string? ownerId)
        {
            var mockRepo = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>());
            var mockExternalBookService = new MockExternalBookService();
            var handler = new CreateBookCommandHandler(mockExternalBookService, mockRepo);
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await handler.Handle(new CreateBookCommand(externalId, ownerId!), CancellationToken.None));
        }
    }
}