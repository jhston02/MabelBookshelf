using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Infrastructure.Interfaces
{
    public interface IEventStoreContext
    {
        Task<T> CreateStreamAsync<T>(T value, string streamName) where T : Entity;
        Task<T> WriteToStreamAsync<T>(T value, string streamName) where T : Entity;
        Task<T> ReadFromStreamAsync<T>(string streamName) where T : Entity;
        Task<bool> StreamExists(string streamId);
    }
}