using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Models;
using MabelBookshelf.Bookshelf.Infrastructure.Models;

namespace MabelBookshelf.Bookshelf.Infrastructure.Interfaces
{
    public interface IProjectionService
    {
        public uint CheckpointInterval { get; }
        public Task<ProjectionPosition> GetCurrentPositionAsync(CancellationToken token = default);
        public Task ProjectAsync(StreamEntry @event, CancellationToken token = default);
        public Task CheckpointAsync(ProjectionPosition position, CancellationToken token = default);
    }
}