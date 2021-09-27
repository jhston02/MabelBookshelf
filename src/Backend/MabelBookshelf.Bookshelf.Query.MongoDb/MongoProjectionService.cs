using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Query.Interfaces;
using MabelBookshelf.Bookshelf.Query.Models;
using MongoDB.Driver;

namespace MabelBookshelf.Bookshelf.Query.MongoDb
{
    public abstract class MongoProjectionService : IProjectionService
    {
        private const string PositionKey = "position";
        private readonly IMongoCollection<IdentifiableProjectionPosition> positionCollection;

        public MongoProjectionService(MongoClient client, MongoProjectionConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.DatabaseName);
            positionCollection = database.GetCollection<IdentifiableProjectionPosition>("projection_position");
        }
        
        public virtual uint CheckpointInterval { get; } = 32;
        public async Task<ProjectionPosition> GetCurrentPositionAsync(CancellationToken token = default)
        {
            var filter = Builders<IdentifiableProjectionPosition>.Filter.Eq(x => x.Id, PositionKey);
            var cursor = await positionCollection.FindAsync(filter, cancellationToken: token);
            var result = cursor.FirstOrDefault();
            return result;
        }

        public abstract Task ProjectAsync(StreamEntry @event, CancellationToken token = default);

        public async Task CheckpointAsync(ProjectionPosition position, CancellationToken token = default)
        {
            var filter = Builders<IdentifiableProjectionPosition>.Filter.Eq(x => x.Id, PositionKey);
            var options = new ReplaceOptions { IsUpsert = true };
            await positionCollection.ReplaceOneAsync(filter,
                new IdentifiableProjectionPosition(PositionKey, position.CommitPosition, position.PreparePosition),
                options,
                token);
        }
        
        private record IdentifiableProjectionPosition
            (string Id, ulong CommitPosition, ulong PreparePosition) : ProjectionPosition(CommitPosition,
                PreparePosition);
    }
}