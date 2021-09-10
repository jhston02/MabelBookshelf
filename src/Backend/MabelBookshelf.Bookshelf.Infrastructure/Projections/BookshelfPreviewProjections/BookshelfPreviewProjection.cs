using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Application.Models;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace MabelBookshelf.Bookshelf.Infrastructure.Projections.BookshelfPreviewProjections
{
    public class BookshelfPreviewProjection : IProjectionService
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<IdentifiableProjectionPosition> positionCollection;
        private readonly IMongoCollection<ChronologicalBookshelfPreview> previewCollection;
        private const string PositionKey = "position";
        public uint CheckpointInterval => 32;

        public BookshelfPreviewProjection(MongoClient client, BookshelfPreviewProjectionConfiguration configuration)
        {
            database = client.GetDatabase(configuration.DatabaseName + $"_v{configuration.Version}");
            positionCollection = database.GetCollection<IdentifiableProjectionPosition>("projection_position");
            previewCollection = database.GetCollection<ChronologicalBookshelfPreview>("bookshelf_preview");
        }
        
        public async Task<ProjectionPosition> GetCurrentPositionAsync(CancellationToken token = default)
        {
            var filter = Builders<IdentifiableProjectionPosition>.Filter.Eq("_id", PositionKey);
            var cursor =  await positionCollection.FindAsync(filter, cancellationToken: token);
            var result = cursor.FirstOrDefault();
            return result;
        }

        public async Task ProjectAsync(StreamEntry @event, CancellationToken token = default)
        {
            switch (@event.DomainEvent)
            {
                case BookCreatedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case BookshelfCreatedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case RenamedBookshelfDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case BookshelfDeletedDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case AddedBookToBookshelfDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
                case RemovedBookFromBookshelfDomainEvent domainEvent:
                    await Apply(domainEvent, @event.StreamPosition);
                    break;
            }
        }

        public async Task CheckpointAsync(ProjectionPosition position, CancellationToken token = default)
        {
            var filter = Builders<IdentifiableProjectionPosition>.Filter.Eq("_id", PositionKey);
            await positionCollection.ReplaceOneAsync(filter, new IdentifiableProjectionPosition(PositionKey, position.CommitPosition, position.PreparePosition), cancellationToken: token);
        }

        #region apply

        private async Task Apply(BookCreatedDomainEvent domainEvent, ulong streamPosition)
        {
            //Every owner has a secret masterlist bookshelf from which all other bookshelves can pull information
            var book = new BookPreview() { ExternalBookId = domainEvent.ExternalId, Categories = domainEvent.Categories.ToList(), BookId = domainEvent.BookId };
            //Note we are not checking the streamPosition because this is strictly additive. 
            //If it was not we would have to get very fancy indeed
            var filterDefinition = Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.OwnerId);
            var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update.AddToSet(x => x.Books, book)
                .SetOnInsert(x => x.StreamPosition, streamPosition)
                .SetOnInsert(x => x.Id, domainEvent.OwnerId)
                .SetOnInsert(x => x.Name, "master")
                .SetOnInsert(x => x.OwnerId, domainEvent.OwnerId);

            var options = new UpdateOptions() {IsUpsert = true};
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition, options);
        }

        private async Task Apply(BookshelfCreatedDomainEvent domainEvent, ulong streamPosition)
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

            var options = new UpdateOptions() {IsUpsert = true};
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition, options);
        }

        private async Task Apply(RenamedBookshelfDomainEvent domainEvent, ulong streamPosition)
        {
            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update.Set(x => x.Name, domainEvent.NewName)
                .Set(x => x.StreamPosition, streamPosition);
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition);
        }

        private async Task Apply(BookshelfDeletedDomainEvent domainEvent, ulong streamPosition)
        {
            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            await previewCollection.DeleteOneAsync(filterDefinition);
        }

        private async Task Apply(AddedBookToBookshelfDomainEvent domainEvent, ulong streamPosition)
        {
            //Pull from master list for owner
            var id = domainEvent.OwnerId.ToString();
            var book = await previewCollection.AsQueryable()
                .Where(x => x.Id == id && x.StreamPosition < streamPosition).SelectMany(x => x.Books)
                .FirstOrDefaultAsync(x => x.BookId == domainEvent.BookId);
            
            //If we have this book (which we should add it to specific shelf
            if (book != null)
            {
                var filterDefinition =
                    Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                    & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

                var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update.Push(x => x.Books, book)
                    .Set(x => x.StreamPosition, streamPosition);
                await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition);
            }
        }
        
        private async Task Apply(RemovedBookFromBookshelfDomainEvent domainEvent, ulong streamPosition)
        {
            var filterDefinition =
                Builders<ChronologicalBookshelfPreview>.Filter.Eq(p => p.Id, domainEvent.BookshelfId.ToString())
                & Builders<ChronologicalBookshelfPreview>.Filter.Lt(p => p.StreamPosition, streamPosition);

            var bookFiler = Builders<BookPreview>.Filter.Eq(x => x.BookId, domainEvent.BookId);


                var updateDefinition = Builders<ChronologicalBookshelfPreview>.Update
                .Set(x => x.StreamPosition, streamPosition)
                .PullFilter(x => x.Books, bookFiler);
            await previewCollection.UpdateOneAsync(filterDefinition, updateDefinition);
        }

        #endregion

        #region wrappers

        private class IdentifiableProjectionPosition : ProjectionPosition
        {
            public string Id { get; set; }

            public IdentifiableProjectionPosition(string id, ulong commitPosition, ulong preparePosition) : base(commitPosition, preparePosition)
            {
                Id = id;
            }
        }

        private class ChronologicalBookshelfPreview : BookshelfPreview
        {
            public ulong StreamPosition { get; set; }
        }
        #endregion
    }
    
    public class BookshelfPreviewProjectionConfiguration
    {
        public string DatabaseName { get; set; }
        public int Version { get; set; } = 1;
    }
}