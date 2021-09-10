using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Queries
{
    using BookshelfPreview = MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models.BookshelfPreview;
    
    public class MongoBookshelfPreviewQueries : IBookshelfPreviewQueries
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<BookshelfPreview> previewCollection;
        
        public MongoBookshelfPreviewQueries(MongoClient client, BookshelfPreviewConfiguration configuration)
        {
            database = client.GetDatabase(configuration.DatabaseName + $"_v{configuration.Version}");
            previewCollection = database.GetCollection<BookshelfPreview>(configuration.CollectionName);
        }
        
        public async Task<IEnumerable<BookshelfPreview>> Previews(string ownerId, uint skip, uint take, CancellationToken token = default)
        {
            var filter = Builders<BookshelfPreview>.Filter.Eq(p => p.OwnerId, ownerId);
            var result =previewCollection
                .Find(filter)
                .Skip((int?)skip)
                .Limit((int?)take);
            return await result.ToListAsync(cancellationToken: token);
        }
    }

    public class MongoBookshelfPreviewQueriesConfiguration
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    }
}