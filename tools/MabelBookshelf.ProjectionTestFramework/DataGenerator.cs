using System;
using System.Collections.Generic;
using AutoBogus;
using Bogus;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events;

namespace MabelBookshelf.ProjectionTestFramework
{
    public class DataGenerator
    {
        string[] authors = new[] { "Tolkien", "Herbert", "Banks", "Heinlein", "Clark" };
        private string[] categories = new[] { "sci fi", "fantasy", "biography" };
        private Faker<BookCreatedDomainEvent> bookFake;
        private Faker<BookshelfCreatedDomainEvent> bookshelfFake;

        private List<string> bookIds = new List<string>();
        private List<Guid> bookshelfIds = new List<Guid>();
        private List<(Guid, string)> booksOnShelves = new List<(Guid, string)>();
        
        public DataGenerator()
        {
            bookFake = new AutoFaker<BookCreatedDomainEvent>()
                .CustomInstantiator(f => new BookCreatedDomainEvent(
                    f.Random.String2(20),
                    f.Random.String2(30),
                    new[] { f.PickRandom(authors) },
                    f.Random.String2(30),
                    f.Random.String2(20),
                    f.Random.Int(1200),
                    "test",
                    new[] { f.PickRandom(categories) }
                ));

            bookshelfFake = new AutoFaker<BookshelfCreatedDomainEvent>()
                .CustomInstantiator(f => new BookshelfCreatedDomainEvent(
                    Guid.NewGuid(),
                    f.Random.String2(15),
                    "test"
                ));
        }
        
        public IEnumerable<BookCreatedDomainEvent> GetBookCreatedDomainEvents(int count)
        {
            var books = bookFake.Generate(count);
            foreach (var book in books)
            {
                bookIds.Add(book.BookId);
                yield return book;
            }
        }

        public IEnumerable<BookshelfCreatedDomainEvent> GetBookshelfCreatedDomainEvents(int count)
        {
            var bookshelves = bookshelfFake.Generate(count);
            foreach (var bookshelf in bookshelves)
            {
                bookshelfIds.Add(bookshelf.BookshelfId);
                yield return bookshelf;
            }
        }

        public IEnumerable<RenamedBookshelfDomainEvent> GetBookshelfRenamedDomainEvents()
        {
            foreach (var bookshelfId in bookshelfIds)
            {
                yield return new RenamedBookshelfDomainEvent(bookshelfId, "new name", "oldName", "test");
            }
        }

        public IEnumerable<BookshelfDeletedDomainEvent> GetBookshelfDeletedDomainEvents()
        {
            foreach (var bookshelfId in bookshelfIds)
            {
                yield return new BookshelfDeletedDomainEvent(bookshelfId, "test");
            }
        }

        //Not the greatest method but it will do for now
        public IEnumerable<AddedBookToBookshelfDomainEvent> GetAddedBookToBookshelfDomainEvents(int numberPerShelf,
            int totalShelves)
        {
            for(int i =0; i<totalShelves; i++)
            {
                for (int j = 0; j < numberPerShelf; j++)
                {
                    booksOnShelves.Add((bookshelfIds[i], bookIds[j]));
                    yield return new AddedBookToBookshelfDomainEvent(bookshelfIds[i], bookIds[j], "test");
                } 
            }
        }

        public IEnumerable<RemovedBookFromBookshelfDomainEvent> GetRemovedBookFromBookshelfDomainEvents()
        {
            foreach (var combination in booksOnShelves)
            {
                yield return new RemovedBookFromBookshelfDomainEvent(combination.Item1, combination.Item2, "test");
            }
        }
    }
}