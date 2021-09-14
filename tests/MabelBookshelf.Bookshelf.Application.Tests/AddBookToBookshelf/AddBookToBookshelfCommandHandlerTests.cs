using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class AddBookToBookshelfCommandHandlerTests
    {
        [Fact]
        public async Task ValidBookValidBookshelf_BookAddedToBookshelf()
        {
            var id = Guid.NewGuid();
            var bookdId = "hey";
            var repository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>()
                { new Domain.Aggregates.BookshelfAggregate.Bookshelf(id, "test", "test") });
            var command = new AddBookToBookshelfCommand(bookdId, id);
            var commandHandler = new AddBookToBookshelfCommandHandler(repository);
            var result = await commandHandler.Handle(command, CancellationToken.None);
            var bookshelf = repository.Bookshelves.FirstOrDefault();
            Assert.True(bookshelf?.Books.FirstOrDefault(x=> x == bookdId) != null);
        }
        
        [Fact]
        public async Task ValidBookInValidBookshelf_BookshelfNotAdded_ThrowsArgumentException()
        {
            var id = Guid.NewGuid();
            var bookdId = "hey";
            var repository = new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>());
            var command = new AddBookToBookshelfCommand(bookdId, id);
            var commandHandler = new AddBookToBookshelfCommandHandler(repository);
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await commandHandler.Handle(command, CancellationToken.None));
        }
    }
}