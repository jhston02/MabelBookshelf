using System;
using System.Collections.Generic;
using System.Linq;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Domain.Tests
{
    using Bookshelf=Domain.Aggregates.BookshelfAggregate.Bookshelf;
    
    public class BookshelfTests
    {
        private Bookshelf GetBookshelf(string name)
        {
            return new Bookshelf(Guid.NewGuid(),name, 0);
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
            var bookGuid = Guid.NewGuid();
            var bookshelf = GetBookshelf("test");
            bookshelf.AddBook(bookGuid);
            Assert.Equal(bookGuid, bookshelf.Books.First());
        }
        
        [Fact]
        public void BookshelfHasBook_RemoveBook_BookshelfDoesNotHaveBook()
        {
            var bookGuid = Guid.NewGuid();
            var bookshelf = GetBookshelf("test");
            bookshelf.AddBook(bookGuid);
            bookshelf.RemoveBook(bookGuid);
            Assert.Empty(bookshelf.Books);
        }

        [Fact]
        public void BookshelfDoesNotHaveBook_RemoveBook_ThrowsException()
        {
            var bookshelf = GetBookshelf("test");
            Assert.Throws<BookshelfDomainException>(() => bookshelf.RemoveBook(Guid.NewGuid()));
        }
        
        [Fact]
        public void BookshelfHasBook_AddKnownBook_ThrowsException()
        {
            var bookGuid = Guid.NewGuid();
            var bookshelf = GetBookshelf("test");
            bookshelf.AddBook(bookGuid);
            Assert.Throws<BookshelfDomainException>(() => bookshelf.AddBook(bookGuid));
        }
    }
}