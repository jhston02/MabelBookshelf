using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    public class Book : AggregateRoot<string>
    {
        public Book(string? id, string? ownerId, VolumeInfo info)
        {
            var @event = new BookCreatedDomainEvent(id ?? throw new ArgumentNullException(nameof(id)), info.Title,
                info.Authors, info.Isbn, info.ExternalId, info.TotalPages,
                ownerId ?? throw new ArgumentNullException(nameof(ownerId)),
                info.Categories);
            When(@event);
        }

        protected Book()
        {
        }

        public VolumeInfo VolumeInfo { get; private set; } = null!;
        public BookStatus Status { get; private set; }
        public int CurrentPageNumber { get; private set; }
        public string OwnerId { get; private set; } = null!;

        public void StartReading()
        {
            if (Status == BookStatus.Reading)
                throw new BookDomainException("Already reading this book");

            var @event = new BookStartedDomainEvent(Id, OwnerId);
            When(@event);
        }

        public void FinishReading()
        {
            if (Status == BookStatus.Finished)
                throw new BookDomainException("Already finished this book");

            if (Status == BookStatus.Dnf)
                throw new BookDomainException("This book was not finished");

            var @event = new BookFinishedDomainEvent(Id, OwnerId);
            When(@event);
        }

        public void MarkAsNotFinished()
        {
            var @event = new NotFinishDomainEvent(Id, OwnerId);
            When(@event);
        }

        public void MarkBookAsWanted()
        {
            var @event = new MarkedBookAsWantedDomainEvent(Id, OwnerId);
            When(@event);
        }

        public void ReadToPage(int pageNumber)
        {
            if (Status == BookStatus.Dnf || Status == BookStatus.Finished)
                throw new BookDomainException($"Cannot change page number if Book is {Status}");

            if (pageNumber <= 0 || pageNumber > VolumeInfo.TotalPages)
                throw new BookDomainException("Please enter valid page number");

            if (Status != BookStatus.Reading)
                StartReading();

            var @event = new ReadToPageDomainEvent(Id, CurrentPageNumber, pageNumber, OwnerId);
            When(@event);

            if (CurrentPageNumber == VolumeInfo.TotalPages)
                FinishReading();
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new ArgumentException($"Book {VolumeInfo.Title} is already deleted");
            var @event = new BookDeletedDomainEvent(Id, OwnerId);
            When(@event);
        }

        #region Apply Event

        public override void Apply(DomainEvent @event)
        {
            switch (@event)
            {
                case BookCreatedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case BookFinishedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case BookStartedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case NotFinishDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case MarkedBookAsWantedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case ReadToPageDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                case BookDeletedDomainEvent domainEvent:
                    Apply(domainEvent);
                    break;
                default:
                    throw new ArgumentException("Invalid event type");
            }

            Version++;
        }

        private void Apply(NotFinishDomainEvent @event)
        {
            Status = BookStatus.Dnf;
        }

        private void Apply(BookFinishedDomainEvent @event)
        {
            Status = BookStatus.Finished;
        }

        private void Apply(BookStartedDomainEvent @event)
        {
            Status = BookStatus.Reading;
        }

        private void Apply(MarkedBookAsWantedDomainEvent @event)
        {
            Status = BookStatus.Want;
        }

        private void Apply(ReadToPageDomainEvent @event)
        {
            CurrentPageNumber = @event.NewPageNumber;
        }

        private void Apply(BookCreatedDomainEvent @event)
        {
            Id = @event.BookId;
            VolumeInfo = new VolumeInfo(@event.Title, @event.Authors, @event.Isbn, @event.ExternalId, @event.TotalPages,
                @event.Categories);
            Status = BookStatus.Want;
            OwnerId = @event.OwnerId;
            IsDeleted = false;
            CurrentPageNumber = 0;
        }

        private void Apply(BookDeletedDomainEvent @event)
        {
            IsDeleted = true;
        }

        #endregion
    }
}