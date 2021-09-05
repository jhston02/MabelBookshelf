using System;
using System.Linq;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Domain.Tests
{
    public class BookshelfTests
    {
        private Aggregates.BookshelfAggregate.Bookshelf GetBookshelf(string name)
        {
            return new Aggregates.BookshelfAggregate.Bookshelf(Guid.NewGuid(), name, 0.ToString());
        }

        [Fact]
        public void Bookshelf_ChangeName_BookshelfHasNewName()
        {
            var bookshelf = GetBookshelf("test");
            bookshelf.Rename("new");
            Assert.Equal("new", bookshelf.Name);
        }

        [Fact]
        public void Bookshelf_AddBook_BookshelfHasBook()
        {
            var bookGuid = Guid.NewGuid().ToString();
            var bookshelf = GetBookshelf("test");
            bookshelf.AddBook(bookGuid);
            Assert.Equal(bookGuid, bookshelf.Books.First());
        }

        [Fact]
        public void BookshelfHasBook_RemoveBook_BookshelfDoesNotHaveBook()
        {
            var bookGuid = Guid.NewGuid().ToString();
            var bookshelf = GetBookshelf("test");
            bookshelf.AddBook(bookGuid);
            bookshelf.RemoveBook(bookGuid);
            Assert.Empty(bookshelf.Books);
        }

        [Fact]
        public void BookshelfDoesNotHaveBook_RemoveBook_ThrowsException()
        {
            var bookshelf = GetBookshelf("test");
            Assert.Throws<BookshelfDomainException>(() => bookshelf.RemoveBook(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void BookshelfHasBook_AddKnownBook_ThrowsException()
        {
            var bookGuid = Guid.NewGuid().ToString();
            var bookshelf = GetBookshelf("test");
            bookshelf.AddBook(bookGuid);
            Assert.Throws<BookshelfDomainException>(() => bookshelf.AddBook(bookGuid));
        }
    }
}