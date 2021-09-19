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
    public class FinishBookCommandHandlerTests
    {
        [Fact]
        public async Task FinishBookCommandHandler_ValidCommand_BookStatusFinished()
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

            var handler = new FinishBookCommandHandler(mockRepo);
            await handler.Handle(new FinishBookCommand("test"), CancellationToken.None);
            var book = mockRepo.Books.FirstOrDefault(x => x.VolumeInfo.ExternalId == "test");
            Assert.Equal(BookStatus.Finished, book.Status);
        }
    }
}