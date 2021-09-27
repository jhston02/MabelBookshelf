using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Query.Models;

namespace MabelBookshelf.Bookshelf.Query.Interfaces
{
    public interface IProjectionService
    {
        public uint CheckpointInterval { get; }
        public Task<ProjectionPosition> GetCurrentPositionAsync(CancellationToken token = default);
        public Task ProjectAsync(StreamEntry @event, CancellationToken token = default);
        public Task CheckpointAsync(ProjectionPosition position, CancellationToken token = default);
    }
}