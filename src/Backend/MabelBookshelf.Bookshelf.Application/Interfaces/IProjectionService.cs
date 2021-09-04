using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Models;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Interfaces
{
    public interface IProjectionService
    {
        public uint CheckpointInterval { get; }
        public Task<ProjectionPosition> GetCurrentPosition();
        public Task Project(StreamEntry @event);
        public Task Checkpoint(ProjectionPosition position);
    }
}