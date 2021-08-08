using System;
using System.Collections.Generic;
using MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public class Bookshelf : Entity<Guid>
    {
        public string Name { get; private set; }
        private List<Guid> _booksIds;
        public IReadOnlyCollection<Guid> Books => _booksIds;
        public long OwnerId { get; private set; }

        public Bookshelf(string name, long ownerId)
        {
            _booksIds = new List<Guid>();
            OwnerId = ownerId;
            Name = name;
        }
        
        public void AddBook(Guid bookId)
        {
            if (_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book already in bookshelf");

            var @event = new AddedBookToBookshelf(this.Id, bookId, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void RemoveBook(Guid bookId)
        {
            if (!_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book not in bookshelf");

            var @event = new RemovedBookFromBookshelf(this.Id, bookId, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void Rename(string newName)
        {
            var @event = new RenamedBookshelf(this.Id, newName, this.Name, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        #region Apply Events

        public void Apply(AddedBookToBookshelf @event)
        {
            Version++;
            this._booksIds.Add(@event.BookId);
        }
        
        public void Apply(RemovedBookFromBookshelf @event)
        {
            Version++;
            this._booksIds.Remove(@event.BookId);
        }
        
        public void Apply(RenamedBookshelf @event)
        {
            Version++;
            this.Name = @event.NewName;
        }
        #endregion
    }
}