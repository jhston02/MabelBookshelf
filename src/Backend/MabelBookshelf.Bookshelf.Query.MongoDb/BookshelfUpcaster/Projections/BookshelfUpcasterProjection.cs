using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Query.Events;
using MabelBookshelf.Bookshelf.Query.Interfaces;
using MabelBookshelf.Bookshelf.Query.Models;
using MabelBookshelf.Bookshelf.Query.MongoDb.BookshelfUpcaster.Configuration;
using MabelBookshelf.Bookshelf.Query.MongoDb.BookshelfUpcaster.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MabelBookshelf.Bookshelf.Query.MongoDb.BookshelfUpcaster.Projections
{
    public class BookshelfUpcasterProjection : MongoProjectionService
    {
        private const string StreamName = "Bookshelf-Upcast";
        private readonly IMongoCollection<BookUpcastInfo> upcastInfoCollection;
        private readonly IEventContext eventContext;
        
        public BookshelfUpcasterProjection(MongoClient client, BookshelfUpcasterConfiguration configuration, IEventContext eventContext) : base(client, configuration)
        {
            var database = client.GetDatabase(configuration.DatabaseName);
            upcastInfoCollection = database.GetCollection<BookUpcastInfo>(configuration.CollectionName);
            this.eventContext = eventContext;
        }

        public override uint CheckpointInterval { get; } = 1;

        public override async Task ProjectAsync(StreamEntry @event, CancellationToken token = default)
        {
            switch (@event.DomainEvent)
            {
                case BookCreatedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition, token);
                    break;
                case AddedBookToBookshelfDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case BookFinishedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case BookStartedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case NotFinishDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case MarkedBookAsWantedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
            }
        }

        #region apply

        private async Task Apply(BookCreatedDomainEvent domainEvent, ulong streamPosition, CancellationToken token = default)
        {
            //Note we are not checking the streamPosition because this is strictly additive. 
            //If it was not we would have to get very fancy indeed
            var filterDefinition = Builders<BookUpcastInfo>.Filter.Eq(p => p.Id, domainEvent.BookId);
            var updateDefinition = Builders<BookUpcastInfo>.Update
                .SetOnInsert(x => x.Id, domainEvent.BookId)
                .SetOnInsert(x => x.ExternalId, domainEvent.ExternalId)
                .SetOnInsert(x => x.Categories, domainEvent.Categories)
                .SetOnInsert(x => x.Authors, domainEvent.Authors)
                .SetOnInsert(x => x.OwnerId, domainEvent.OwnerId)
                .SetOnInsert(x => x.Title, domainEvent.Title)
                .SetOnInsert(x => x.Status, domainEvent.Status)
                .SetOnInsert(x => x.StreamPosition, streamPosition);

            var options = new UpdateOptions { IsUpsert = true };
            await upcastInfoCollection.UpdateOneAsync(filterDefinition, updateDefinition, options, token);
        }

        private async Task Apply(AddedBookToBookshelfDomainEvent domainEvent, ulong streamPosition,
            CancellationToken token = default)
        {
            var book = await upcastInfoCollection.AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == domainEvent.BookId, token);

            var @event = new BookAddedToBookshelfWithBookInfo(book.Id, book.Title, book.Authors, book.ExternalId,
                book.OwnerId, book.Categories, book.Status, domainEvent.BookshelfId);
            
            await eventContext.AppendEvent(StreamName, token, @event);
        }

        private async Task Apply(BookFinishedDomainEvent domainEvent, ulong streamPosition,
            CancellationToken token = default)
        {
            var filterDefinition =
                Builders<BookUpcastInfo>.Filter.Eq(p => p.Id, domainEvent.BookId.ToString())
                & Builders<BookUpcastInfo>.Filter.Lt(p => p.StreamPosition, streamPosition);


            var updateDefinition = Builders<BookUpcastInfo>.Update
                .Set(x => x.StreamPosition, streamPosition)
                .Set(x => x.Status, domainEvent.Status);
            await upcastInfoCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }
        
        private async Task Apply(BookStartedDomainEvent domainEvent, ulong streamPosition,
            CancellationToken token = default)
        {
            var filterDefinition =
                Builders<BookUpcastInfo>.Filter.Eq(p => p.Id, domainEvent.BookId.ToString())
                & Builders<BookUpcastInfo>.Filter.Lt(p => p.StreamPosition, streamPosition);


            var updateDefinition = Builders<BookUpcastInfo>.Update
                .Set(x => x.StreamPosition, streamPosition)
                .Set(x => x.Status, domainEvent.Status);
            await upcastInfoCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }
        
        private async Task Apply(NotFinishDomainEvent domainEvent, ulong streamPosition,
            CancellationToken token = default)
        {
            var filterDefinition =
                Builders<BookUpcastInfo>.Filter.Eq(p => p.Id, domainEvent.BookId.ToString())
                & Builders<BookUpcastInfo>.Filter.Lt(p => p.StreamPosition, streamPosition);


            var updateDefinition = Builders<BookUpcastInfo>.Update
                .Set(x => x.StreamPosition, streamPosition)
                .Set(x => x.Status, domainEvent.Status);
            await upcastInfoCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }
        
        private async Task Apply(MarkedBookAsWantedDomainEvent domainEvent, ulong streamPosition,
            CancellationToken token = default)
        {
            var filterDefinition =
                Builders<BookUpcastInfo>.Filter.Eq(p => p.Id, domainEvent.BookId.ToString())
                & Builders<BookUpcastInfo>.Filter.Lt(p => p.StreamPosition, streamPosition);


            var updateDefinition = Builders<BookUpcastInfo>.Update
                .Set(x => x.StreamPosition, streamPosition)
                .Set(x => x.Status, domainEvent.Status);
            await upcastInfoCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }
        #endregion
    }
}