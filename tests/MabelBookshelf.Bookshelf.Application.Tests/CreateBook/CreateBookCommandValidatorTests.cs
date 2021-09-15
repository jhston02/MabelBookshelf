using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class CreateBookCommandValidatorTests
    {
        [Fact]
        public void ValidCreateBookCommand_IsValid()
        {
            var mockExternalBookService = new MockExternalBookService();
            var command = new CreateBookCommand("test", "blah");
            var validator = new CreateBookCommandValidator(mockExternalBookService);
            Assert.True(validator.Validate(command).IsValid);
        }

        [Fact]
        public void InvalidCreateBookCommand_WrongExternalId_IsInvalid()
        {
            var mockExternalBookService = new MockExternalBookService();
            var command = new CreateBookCommand("bad", "blah");
            var validator = new CreateBookCommandValidator(mockExternalBookService);
            Assert.False(validator.Validate(command).IsValid);
        }
    }
}