using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfCreatedDomainEvent : DomainEvent
    {
        public string Name { get; private set; }
        public string OwnerId { get; private set; }
        
        public BookshelfCreatedDomainEvent(string streamId, string name, string ownerId, long streamPosition) : base(streamId, streamPosition)
        {
            this.Name = name;
            this.OwnerId = ownerId;
        }
    }
}