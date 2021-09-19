using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class MarkAsNotFinishedCommandHandlerTests
    {
        [Fact]
        public async Task MarkAsNotFinishedCommandHandler_ValidCommand_BookStatusNotFinished()
        {
            var mockExternalBookService = new MockExternalBookService();
            var mockRepo =
                new MockBookRepository(
                    new List<Domain.Aggregates.BookAggregate.Book>
                    {
                        new("test",
                            "test", await VolumeInfo.FromExternalId("test", mockExternalBookService)
                        )
                    });

            var handler = new MarkAsNotFinishedCommandHandler(mockRepo);
            await handler.Handle(new MarkAsNotFinishedCommand("test"), CancellationToken.None);
            var book = mockRepo.Books.FirstOrDefault(x => x.VolumeInfo.ExternalId == "test");
            Assert.Equal(BookStatus.Dnf, book.Status);
        }
    }
}