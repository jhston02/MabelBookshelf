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
            var @event = new BookshelfCreatedDomainEvent(id, name, ownerId);
            When(@event);
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

            var @event = new AddedBookToBookshelfDomainEvent(Id, bookId);
            When(@event);
        }

        public void RemoveBook(string bookId)
        {
            if (!_booksIds.Contains(bookId))
                throw new BookshelfDomainException("Book not in bookshelf");

            var @event = new RemovedBookFromBookshelfDomainEvent(Id, bookId);
            When(@event);
        }

        public void Rename(string newName)
        {
            var @event = new RenamedBookshelfDomainEvent(Id, newName, Name);
            When(@event);
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new ArgumentException($"Bookshelf {Name} is already deleted");
            var @event = new BookshelfDeletedDomainEvent(Id);
            When(@event);
        }

        #region Apply Event

        public override void Apply(DomainEvent @event)
        {
            switch (@event)
            {
                case AddedBookToBookshelfDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case BookshelfCreatedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case RemovedBookFromBookshelfDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case RenamedBookshelfDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case BookshelfDeletedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                default:
                    throw new ArgumentException("Invalid event type");
            }

            Version++;
        }

        private void Apply(AddedBookToBookshelfDomainEvent @event)
        {
            _booksIds.Add(@event.BookId);
        }

        private void Apply(RemovedBookFromBookshelfDomainEvent @event)
        {
            _booksIds.Remove(@event.BookId);
        }

        private void Apply(RenamedBookshelfDomainEvent @event)
        {
            Name = @event.NewName;
        }

        private void Apply(BookshelfCreatedDomainEvent @event)
        {
            Id = @event.BookshelfId;
            _booksIds = new List<string>();
            OwnerId = @event.OwnerId;
            Name = @event.Name;
            IsDeleted = false;
        }

        private void Apply(BookshelfDeletedDomainEvent @event)
        {
            IsDeleted = true;
        }

        #endregion
    }
}