using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Query.MongoDb.Configuration;
using MabelBookshelf.Bookshelf.Query.Queries.Preview;
using MabelBookshelf.Bookshelf.Query.Queries.Preview.Models;
using MongoDB.Driver;

namespace MabelBookshelf.Bookshelf.Query.MongoDb.Queries
{
    public class MongoBookshelfPreviewQueries : IBookshelfPreviewQueries
    {
        private readonly IMongoDatabase database;

        private readonly IMongoCollection<BookshelfPreview>
            previewCollection;

        public MongoBookshelfPreviewQueries(MongoClient client, BookshelfPreviewConfiguration configuration)
        {
            database = client.GetDatabase(configuration.DatabaseName);
            previewCollection =
                database.GetCollection<BookshelfPreview>(configuration
                    .CollectionName);
        }

        public async Task<IEnumerable<BookshelfPreview>> Previews(
            string ownerId, uint skip, uint take, CancellationToken token = default)
        {
            var filter =
                Builders<BookshelfPreview>.Filter.Eq(p => p.OwnerId,
                    ownerId);
            var result = previewCollection
                .Find(filter)
                .Skip((int?)skip)
                .Limit((int?)take);
            return await result.ToListAsync(token);
        }
    }
}