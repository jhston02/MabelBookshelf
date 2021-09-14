using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class AddBookToBookshelfCommandValidatorTests
    {
        [Fact]
        public async Task ValidAddBookToBookshelfCommand_IsValid()
        {
            var id = Guid.NewGuid();
            var owner = "test";
            var bookId = "hey";
            var bookRepository = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>()
            {
                new Domain.Aggregates.BookAggregate.Book(bookId, owner,
                    await VolumeInfo.FromExternalId("blah", new MockExternalBookService()))
            });
            var bookshelfRepository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>()
                { new Domain.Aggregates.BookshelfAggregate.Bookshelf(id, "test", "test") });
            var validator = new AddBookToBookshelfCommandValidator(bookRepository, bookshelfRepository);
            var command = new AddBookToBookshelfCommand(bookId, id, owner);
            Assert.True((await validator.ValidateAsync(command)).IsValid);
        }
    }
}