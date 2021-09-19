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
    public class StartReadingCommandHandlerTests
    {
        [Fact]
        public async Task StartReadingCommandHandlerTests_ValidCommand_BookStatusStarted()
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

            var handler = new StartReadingCommandHandler(mockRepo);
            await handler.Handle(new StartReadingCommand("test"), CancellationToken.None);
            var book = mockRepo.Books.FirstOrDefault(x => x.VolumeInfo.ExternalId == "test");
            Assert.Equal(BookStatus.Reading, book.Status);
        }
    }
}