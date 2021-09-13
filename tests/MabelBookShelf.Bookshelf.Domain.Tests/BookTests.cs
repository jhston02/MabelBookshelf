using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Shared;
using Xunit;

namespace MabelBookshelf.Bookshelf.Domain.Tests
{
    public class BookTests
    {
        private Book GetBook(BookStatus status)
        {
            var book = new Book(Guid.NewGuid().ToString(), "0",
                VolumeInfo.FromExternalId("blah", new MockExternalBooksService()).Result);
            if (status == BookStatus.Reading)
                book.ReadToPage(1);
            else if (status == BookStatus.Dnf)
                book.MarkAsNotFinished();
            else if (status == BookStatus.Finished)
                book.FinishReading();
            return book;
        }

        [Fact]
        public void WantedBook_StartReading_StatusIsReading()
        {
            var book = GetBook(BookStatus.Want);
            book.StartReading();
            Assert.Equal(BookStatus.Reading, book.Status);
        }

        [Fact]
        public void WantedBook_Finished_StatusIsFinished()
        {
            var book = GetBook(BookStatus.Want);
            book.StartReading();
            Assert.Equal(BookStatus.Reading, book.Status);
        }

        [Fact]
        public void WantedBook_DNFinished_StatusIsDNFinished()
        {
            var book = GetBook(BookStatus.Want);
            book.MarkAsNotFinished();
            Assert.Equal(BookStatus.Dnf, book.Status);
        }

        [Fact]
        public void DNFBook_GoToFinished_ThrowException()
        {
            var book = GetBook(BookStatus.Dnf);
            Assert.Throws<BookDomainException>(() => book.FinishReading());
        }

        [Fact]
        public void FinishedBook_GoToFinished_ThrowException()
        {
            var book = GetBook(BookStatus.Finished);
            Assert.Throws<BookDomainException>(() => book.FinishReading());
        }

        [Fact]
        public void ReadingBook_GoToReading_ThrowException()
        {
            var book = GetBook(BookStatus.Reading);
            Assert.Throws<BookDomainException>(() => book.StartReading());
        }

        [Fact]
        public void NewBook_ReadPagesLessThanMax_OnPageNumber()
        {
            var book = GetBook(BookStatus.Reading);
            book.ReadToPage(247);
            Assert.Equal(247, book.CurrentPageNumber);
        }

        [Fact]
        public void NewBook_ReadPagesMoreThanMax_ThrowsException()
        {
            var book = GetBook(BookStatus.Reading);
            Assert.Throws<BookDomainException>(() => book.ReadToPage(600));
        }

        [Fact]
        public void NewBook_ReadPagesLessThanZero_ThrowsException()
        {
            var book = GetBook(BookStatus.Reading);
            Assert.Throws<BookDomainException>(() => book.ReadToPage(-5));
        }

        [Fact]
        public void NewBook_ReadPagesEqualToMax_BookFinished()
        {
            var book = GetBook(BookStatus.Reading);
            book.ReadToPage(500);
            Assert.Equal(BookStatus.Finished, book.Status);
        }

        [Fact]
        public void NewBookInWanted_ReadPagesLessThanMax_BookReading()
        {
            var book = GetBook(BookStatus.Want);
            book.ReadToPage(200);
            Assert.Equal(BookStatus.Reading, book.Status);
        }

        private class MockExternalBooksService : IExternalBookService
        {
            public Task<ExternalBook> GetBookAsync(string externalBookId, CancellationToken token = default)
            {
                var book = new ExternalBook("blah", "blah", new[] { "test" }, "blah", 500, new[] { "test" });
                return Task.FromResult(book);
            }
        }
    }
}