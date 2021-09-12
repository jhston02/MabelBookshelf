using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    public record BookDomainEvent(string BookId, string OwnerId) : DomainEvent;
    
    public record BookCreatedDomainEvent(string BookId, string Title, string[] Authors, string Isbn, string ExternalId,
        int TotalPages, string OwnerId, string[] Categories) : BookDomainEvent(BookId, OwnerId);
    
    public record BookDeletedDomainEvent(string BookId, string OwnerId) : BookDomainEvent(BookId, OwnerId);
    
    public record BookFinishedDomainEvent(string BookId, string OwnerId) : BookDomainEvent(BookId, OwnerId);
    
    public record BookStartedDomainEvent(string BookId, string OwnerId) : BookDomainEvent(BookId, OwnerId);
    
    public record MarkedBookAsWantedDomainEvent(string BookId, string OwnerId) : BookDomainEvent(BookId, OwnerId);
    
    public record NotFinishDomainEvent(string BookId, string OwnerId) : BookDomainEvent(BookId, OwnerId);
    
    public record ReadToPageDomainEvent(string BookId, int OldPageNumber, int NewPageNumber, string OwnerId) : BookDomainEvent(BookId, OwnerId);
}