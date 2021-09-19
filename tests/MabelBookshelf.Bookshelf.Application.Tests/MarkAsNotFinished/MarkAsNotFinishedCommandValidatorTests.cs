using System.Collections.Generic;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class MarkAsNotFinishedCommandValidatorTests
    {
        [Fact]
        public async Task ValidMarkBookAsNotFinishedCommand_IsValid()
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

            var command = new MarkAsNotFinishedCommand("test");
            var validator = new MarkAsNotFinishedCommandValidator(mockRepo);
            Assert.True((await validator.ValidateAsync(command)).IsValid);
        }

        [Fact]
        public void InvalidMarkBookAsNotFinished_WrongId_IsInvalid()
        {
            var mockRepo = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>());
            var command = new MarkAsNotFinishedCommand("test-test");
            var validator = new MarkAsNotFinishedCommandValidator(mockRepo);
            Assert.False(validator.Validate(command).IsValid);
        }
    }
}