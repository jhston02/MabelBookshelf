using System;
using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public class Bookshelf : AggregateRoot<Guid>
    {
        private List<string> _booksIds;

        public Bookshelf(Guid id, string name, string ownerId)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var @event = new BookshelfCreatedDomainEvent(id, name, ownerId, Version);
            AddEvent(@event);
            Apply(@event);
        }

        protected Bookshelf()
        {
        }

        public string Name { get; private set; }
        public IReadOnlyCollection<string> Books => _booksIds;
        public string OwnerId { get; private set; }

        public void AddBook(string bookId)
        {
            if (_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book already in bookshelf");

            var @event = new AddedBookToBookshelfDomainEvent(Id, bookId, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public void RemoveBook(string bookId)
        {
            if (!_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book not in bookshelf");

            var @event = new RemovedBookFromBookshelfDomainEvent(Id, bookId, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public void Rename(string newName)
        {
            var @event = new RenamedBookshelfDomainEvent(Id, newName, Name, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new ArgumentException($"Bookshelf {Name} is already deleted");
            var @event = new BookshelfDeletedDomainEvent(Id, Version);
            AddEvent(@event);
            Apply(@event);
        }

        #region Apply Events

        public override void Apply(DomainEvent @event)
        {
            if (@event.StreamPosition == Version)
            {
                if (@event is AddedBookToBookshelfDomainEvent)
                    Apply(@event as AddedBookToBookshelfDomainEvent);
                else if (@event is BookshelfCreatedDomainEvent)
                    Apply(@event as BookshelfCreatedDomainEvent);
                else if (@event is RemovedBookFromBookshelfDomainEvent)
                    Apply(@event as RemovedBookFromBookshelfDomainEvent);
                else if (@event is RenamedBookshelfDomainEvent)
                    Apply(@event as RenamedBookshelfDomainEvent);
                else if (@event is BookshelfDeletedDomainEvent)
                    Apply(@event as BookshelfDeletedDomainEvent);
            }
            else
            {
                throw new ArgumentException("Event is not in order!");
            }
        }

        private void Apply(AddedBookToBookshelfDomainEvent @event)
        {
            Version++;
            _booksIds.Add(@event.BookId);
        }

        private void Apply(RemovedBookFromBookshelfDomainEvent @event)
        {
            Version++;
            _booksIds.Remove(@event.BookId);
        }

        private void Apply(RenamedBookshelfDomainEvent @event)
        {
            Version++;
            Name = @event.NewName;
        }

        private void Apply(BookshelfCreatedDomainEvent @event)
        {
            if (@event.StreamPosition != 0)
                throw new ArgumentException("Cannot create bookshelf twice");

            Version++;
            Id = @event.BookshelfId;
            _booksIds = new List<string>();
            OwnerId = @event.OwnerId;
            Name = @event.Name;
            IsDeleted = false;
        }

        private void Apply(BookshelfDeletedDomainEvent @event)
        {
            Version++;
            IsDeleted = true;
        }

        #endregion
    }
}