using System;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class CreateBookshelfCommandValidatorTests
    {
        [Fact]
        public void ValidCommand_ValidatorReturnsValid()
        {
            var filter = new ProfanityFilter.ProfanityFilter();
            var validator = new CreateBookshelfCommandValidator(filter);
            var result = validator.Validate(new CreateBookshelfCommand(Guid.NewGuid(), "nice name", "test"));
            Assert.True(result.IsValid);
        }

        [Fact]
        public void InValidCommand_ContainsCurseWord_ValidatorReturnsInvalid()
        {
            var filter = new ProfanityFilter.ProfanityFilter();
            var validator = new CreateBookshelfCommandValidator(filter);
            var result = validator.Validate(new CreateBookshelfCommand(Guid.NewGuid(), "shit", "test"));
            Assert.False(result.IsValid);
        }
    }
}