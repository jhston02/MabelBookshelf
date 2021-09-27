using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Query.Events;
using MabelBookshelf.Bookshelf.Query.Models;
using MabelBookshelf.Bookshelf.Query.MongoDb.Configuration;
using MabelBookshelf.Bookshelf.Query.MongoDb.Models;
using MabelBookshelf.Bookshelf.Query.Queries.Preview.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MabelBookshelf.Bookshelf.Query.MongoDb.Projections
{
    public class MongoBookshelfPreviewProjection : MongoProjectionService
    {
        private readonly IMongoCollection<ChronologicalBookshelfPreview> previewCollection;

        public MongoBookshelfPreviewProjection(MongoClient client, BookshelfPreviewConfiguration configuration) : base(client, configuration)
        {
            var database = client.GetDatabase(configuration.DatabaseName);
            previewCollection = database.GetCollection<ChronologicalBookshelfPreview>(configuration.CollectionName);
        }

        public override async Task ProjectAsync(StreamEntry @event, CancellationToken token = default)
        {
            switch (@event.DomainEvent)
            {
                case BookshelfCreatedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition, token);
                    break;
                case RenamedBookshelfDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition, token);
                    break;
                case BookshelfDeletedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition, token);
                    break;
                case BookAddedToBookshelfWithBookInfo domainEvent:
                    await Apply(domainEvent, @event.StreamPosition, token);
                    break;
                case RemovedBookFromBookshelfDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition, token);
                    break;
            }
        }

        #region apply

        private async Task Apply(BookshelfCreatedDomainEvent domainEvent, ulong streamPosition, CancellationToken token = default)
        {
            //Relying on the secret upsert tech is easier than doing the stream position stuff 
            //from what I can tell in Mongo still new so might be coming back to this later
            var id = domainEvent.BookshelfId.ToString();
            var filterDefinition = Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, id);
            var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update
                .SetOnInsert(x => x.StreamPosition, streamPosition)
                .SetOnInsert(x => x.Id, id)
                .SetOnInsert(x => x.Name, domainEvent.Name)
                .SetOnInsert(x => x.Books, new List<BookPreview>())
                .SetOnInsert(x => x.OwnerId, domainEvent.OwnerId);

            var options = new UpdateOptions { IsUpsert = true };
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition, options, token);
        }

        private async Task Apply(RenamedBookshelfDomainEvent domainEvent, ulong streamPosition, CancellationToken token = default)
        {
            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update.Set(x => x.Name, domainEvent.NewName)
                .Set(x => x.StreamPosition, streamPosition);
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }

        private async Task Apply(BookshelfDeletedDomainEvent domainEvent, ulong streamPosition, CancellationToken token = default)
        {
            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            await previewCollection.DeleteOneAsync(filterDefinition, token);
        }

        //This should be upcasted using a fast store. We know we need a similar thing for actual bookshelves so
        //lets make it fast
        private async Task Apply(BookAddedToBookshelfWithBookInfo domainEvent, ulong streamPosition, CancellationToken token = default)
        {

            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update
                .PushEach(x => x.Books, new List<BookPreview> { new BookPreview(domainEvent.BookId, domainEvent.ExternalId, domainEvent.Authors) }, -15)
                .Set(x => x.StreamPosition, streamPosition);
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }

        private async Task Apply(RemovedBookFromBookshelfDomainEvent domainEvent, ulong streamPosition, CancellationToken token = default)
        {
            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            var bookFilter = Builders<BookPreview>.Filter.Eq(x => x.BookId, domainEvent.BookId);


            var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update
                .Set(x => x.StreamPosition, streamPosition)
                .PullFilter(x => x.Books, bookFilter);
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition, cancellationToken: token);
        }

        #endregion
    }
}