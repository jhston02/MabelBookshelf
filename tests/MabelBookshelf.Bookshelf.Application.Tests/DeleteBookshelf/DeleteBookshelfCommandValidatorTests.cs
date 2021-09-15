using System;
using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class DeleteBookshelfCommandValidatorTests
    {
        [Fact]
        public void ValidDeleteBookshelfCommand_IsValid()
        {
            var id = Guid.NewGuid();
            var owner = "test";
            var repository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>
                { new(id, "test", owner) });
            var validator = new DeleteBookshelfCommandValidator(repository);
            var command = new DeleteBookshelfCommand(id);
            Assert.True(validator.Validate(command).IsValid);
        }

        [Fact]
        public void InvalidDeleteBookshelfCommand_NotBookshelfId_IsInvalid()
        {
            var id = Guid.NewGuid();
            var repository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>());
            var validator = new DeleteBookshelfCommandValidator(repository);
            var command = new DeleteBookshelfCommand(id);
            Assert.False(validator.Validate(command).IsValid);
        }
    }
}