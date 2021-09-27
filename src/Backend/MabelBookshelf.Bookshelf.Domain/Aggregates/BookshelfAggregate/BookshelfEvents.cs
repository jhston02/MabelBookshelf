using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    public abstract record BookshelfDomainEvent(Guid BookshelfId, string OwnerId) : DomainEvent;

    public record AddedBookToBookshelfDomainEvent
        (Guid BookshelfId, string BookId, string OwnerId) : BookshelfDomainEvent(BookshelfId, OwnerId);

    public record BookshelfDeletedDomainEvent(Guid BookshelfId, string OwnerId) : BookshelfDomainEvent(BookshelfId,
        OwnerId);

    public record BookshelfCreatedDomainEvent(Guid BookshelfId, string Name, string OwnerId) : BookshelfDomainEvent(
        BookshelfId, OwnerId);

    public record RemovedBookFromBookshelfDomainEvent
        (Guid BookshelfId, string BookId, string OwnerId) : BookshelfDomainEvent(BookshelfId, OwnerId);

    public record RenamedBookshelfDomainEvent
        (Guid BookshelfId, string NewName, string OldName, string OwnerId) : BookshelfDomainEvent(BookshelfId, OwnerId);
}