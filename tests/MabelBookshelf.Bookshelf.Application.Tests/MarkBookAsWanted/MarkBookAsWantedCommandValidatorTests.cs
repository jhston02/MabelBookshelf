using System.Collections.Generic;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class MarkBookAsWantedCommandValidatorTests
    {
        [Fact]
        public async Task ValidMarkBookAsWantedCommand_IsValid()
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

            var command = new MarkBookAsWantedCommand("test");
            var validator = new MarkBookAsWantedCommandValidator(mockRepo);
            Assert.True((await validator.ValidateAsync(command)).IsValid);
        }

        [Fact]
        public void InvalidMarkBookAsWantedCommand_WrongId_IsInvalid()
        {
            var mockRepo = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>());
            var command = new MarkBookAsWantedCommand("test-test");
            var validator = new MarkBookAsWantedCommandValidator(mockRepo);
            Assert.False(validator.Validate(command).IsValid);
        }
    }
}