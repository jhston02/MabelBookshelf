using System;
using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public class Bookshelf : Entity<Guid>
    {
        public string Name { get; private set; }
        private List<Guid> _booksIds;
        public IReadOnlyCollection<Guid> Books => _booksIds;
        public string OwnerId { get; private set; }

        public Bookshelf(Guid id, string name, string ownerId)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var @event = new BookshelfCreatedDomainEvent(id, name, ownerId, Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }
        
        protected Bookshelf(){ }
        
        public void AddBook(Guid bookId)
        {
            if (_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book already in bookshelf");

            var @event = new AddedBookToBookshelfDomainEvent(this.Id, bookId, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void RemoveBook(Guid bookId)
        {
            if (!_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book not in bookshelf");

            var @event = new RemovedBookFromBookshelfDomainEvent(this.Id, bookId, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void Rename(string newName)
        {
            var @event = new RenamedBookshelfDomainEvent(this.Id, newName, this.Name, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        #region Apply Events

        public override void Apply(DomainEvent<Guid> @event)
        {
            if (@event.StreamPosition == Version)
            {
                if (@event is AddedBookToBookshelfDomainEvent)
                    Apply(@event as AddedBookToBookshelfDomainEvent);
                else if(@event is BookshelfCreatedDomainEvent)
                    Apply(@event as BookshelfCreatedDomainEvent);
                else if(@event is RemovedBookFromBookshelfDomainEvent)
                    Apply(@event as RemovedBookFromBookshelfDomainEvent);
                else if(@event is RenamedBookshelfDomainEvent)
                    Apply(@event as RenamedBookshelfDomainEvent);
            }
            else
            {
                throw new ArgumentException("Event is not in order!");
            }
        }

        public void Apply(AddedBookToBookshelfDomainEvent @event)
        {
            Version++;
            this._booksIds.Add(@event.BookId);
        }
        
        public void Apply(RemovedBookFromBookshelfDomainEvent @event)
        {
            Version++;
            this._booksIds.Remove(@event.BookId);
        }
        
        public void Apply(RenamedBookshelfDomainEvent @event)
        {
            Version++;
            this.Name = @event.NewName;
        }

        public void Apply(BookshelfCreatedDomainEvent @event)
        {
            if (@event.StreamPosition != 0)
                throw new ArgumentException("Cannot create bookshelf twice");
            Version++;
            Id = @event.StreamId;
            _booksIds = new List<Guid>();
            OwnerId = @event.OwnerId;
            Name = @event.Name;
        }
        #endregion
    }
}