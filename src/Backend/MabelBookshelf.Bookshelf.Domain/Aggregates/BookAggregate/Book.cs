using System;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    //TODO: check isbn format
    public class Book : AggregateRoot<string>
    {
        public Book(string id, string title, string[] authors, string isbn, string externalId, int totalPages,
            string ownerId, string[] categories)
        {
            var @event = new BookCreatedDomainEvent(id, title, authors, isbn, externalId, totalPages, Version, ownerId,
                categories);
            AddEvent(@event);
            Apply(@event);
        }

        protected Book()
        {
        }

        public string Title { get; private set; }
        public string[] Authors { get; private set; }
        public string Isbn { get; private set; }
        public string ExternalId { get; private set; }
        public BookStatus Status { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentPageNumber { get; private set; }
        public string OwnerId { get; private set; }
        public string[] Categories { get; private set; }

        public void StartReading()
        {
            if (Status == BookStatus.Reading)
                throw new BookDomainException("Already reading this book");

            var @event = new BookStartedDomainEvent(Id, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public void FinishReading()
        {
            if (Status == BookStatus.Finished)
                throw new BookDomainException("Already finished this book");

            if (Status == BookStatus.Dnf)
                throw new BookDomainException("This book was not finished");

            var @event = new BookFinishedDomainEvent(Id, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public void MarkAsNotFinished()
        {
            if (Status == BookStatus.Finished)
                throw new BookDomainException("Already finished this book");
            var @event = new NotFinishDomainEvent(Id, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public void MarkBookAsWanted()
        {
            var @event = new MarkedBookAsWantedDomainEvent(Id, Version);
            AddEvent(@event);
            Apply(@event);
        }

        public void ReadToPage(int pageNumber)
        {
            if (pageNumber <= 0 || pageNumber > TotalPages)
                throw new BookDomainException("Please enter valid page number");

            if (Status != BookStatus.Reading)
                StartReading();

            var @event = new ReadToPageDomainEvent(Id, CurrentPageNumber, pageNumber, Version);
            AddEvent(@event);
            Apply(@event);

            if (CurrentPageNumber == TotalPages)
                FinishReading();
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new ArgumentException($"Book {Title} is already deleted");
            var @event = new BookDeletedDomainEvent(Id, Version);
            AddEvent(@event);
            Apply(@event);
        }

        #region Apply Events

        public override void Apply(DomainEvent @event)
        {
            if (@event.StreamPosition == Version)
            {
                if (@event is BookCreatedDomainEvent)
                    Apply(@event as BookCreatedDomainEvent);
                else if (@event is BookFinishedDomainEvent)
                    Apply(@event as BookFinishedDomainEvent);
                else if (@event is BookStartedDomainEvent)
                    Apply(@event as BookStartedDomainEvent);
                else if (@event is NotFinishDomainEvent)
                    Apply(@event as NotFinishDomainEvent);
                else if (@event is MarkedBookAsWantedDomainEvent)
                    Apply(@event as MarkedBookAsWantedDomainEvent);
                else if (@event is ReadToPageDomainEvent)
                    Apply(@event as ReadToPageDomainEvent);
                else if (@event is BookDeletedDomainEvent)
                    Apply(@event as BookDeletedDomainEvent);
            }
            else
            {
                throw new ArgumentException("Event is not in order!");
            }
        }

        private void Apply(NotFinishDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Dnf;
        }

        private void Apply(BookFinishedDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Finished;
        }

        private void Apply(BookStartedDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Reading;
        }

        private void Apply(MarkedBookAsWantedDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Want;
        }

        private void Apply(ReadToPageDomainEvent @event)
        {
            Version++;
            CurrentPageNumber = @event.NewPageNumber;
        }

        private void Apply(BookCreatedDomainEvent @event)
        {
            if (@event.StreamPosition != 0)
                throw new ArgumentException("Cannot create book twice");
            Version++;
            Id = @event.BookId;
            Title = @event.Title;
            Authors = @event.Authors;
            Isbn = @event.Isbn;
            ExternalId = @event.ExternalId;
            TotalPages = @event.TotalPages;
            Status = BookStatus.Want;
            OwnerId = @event.OwnerId;
            Categories = @event.Categories;
            IsDeleted = false;
            CurrentPageNumber = 0;
        }

        private void Apply(BookDeletedDomainEvent @event)
        {
            Version++;
            IsDeleted = true;
        }

        #endregion
    }
}