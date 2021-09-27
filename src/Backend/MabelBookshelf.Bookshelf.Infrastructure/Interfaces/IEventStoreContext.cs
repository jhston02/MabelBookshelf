using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Infrastructure.Interfaces
{
    public interface IEventStoreContext
    {
        Task<T> CreateStreamAsync<T, TV>(T value, string streamName, CancellationToken token = default)
            where T : AggregateRoot<TV>;

        Task<T> WriteToStreamAsync<T, TV>(T value, string streamName, CancellationToken token = default)
            where T : AggregateRoot<TV>;

        Task<T?> ReadFromStreamAsync<T, TV>(string streamName, CancellationToken token = default)
            where T : AggregateRoot<TV>;

        Task<bool> StreamExists(string streamId, CancellationToken token = default);
    }
}