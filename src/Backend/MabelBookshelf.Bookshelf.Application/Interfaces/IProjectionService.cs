using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Models;

namespace MabelBookshelf.Bookshelf.Application.Interfaces
{
    public interface IProjectionService
    {
        public uint CheckpointInterval { get; }
        public Task<ProjectionPosition> GetCurrentPositionAsync(CancellationToken token = default);
        public Task ProjectAsync(StreamEntry @event, CancellationToken token = default);
        public Task CheckpointAsync(ProjectionPosition position, CancellationToken token = default);
    }
}