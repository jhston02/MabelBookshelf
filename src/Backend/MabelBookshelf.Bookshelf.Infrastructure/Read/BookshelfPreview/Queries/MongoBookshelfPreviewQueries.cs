using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview;
using MongoDB.Driver;

namespace MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Queries
{
    public class MongoBookshelfPreviewQueries : IBookshelfPreviewQueries
    {
        private readonly IMongoDatabase database;

        private readonly IMongoCollection<Application.Bookshelf.Queries.Preview.Models.BookshelfPreview>
            previewCollection;

        public MongoBookshelfPreviewQueries(MongoClient client, BookshelfPreviewConfiguration configuration)
        {
            database = client.GetDatabase(configuration.DatabaseName + $"_v{configuration.Version}");
            previewCollection =
                database.GetCollection<Application.Bookshelf.Queries.Preview.Models.BookshelfPreview>(configuration
                    .CollectionName);
        }

        public async Task<IEnumerable<Application.Bookshelf.Queries.Preview.Models.BookshelfPreview>> Previews(
            string ownerId, uint skip, uint take, CancellationToken token = default)
        {
            var filter =
                Builders<Application.Bookshelf.Queries.Preview.Models.BookshelfPreview>.Filter.Eq(p => p.OwnerId,
                    ownerId);
            var result = previewCollection
                .Find(filter)
                .Skip((int?)skip)
                .Limit((int?)take);
            return await result.ToListAsync(token);
        }
    }

    public class MongoBookshelfPreviewQueriesConfiguration
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    }
}