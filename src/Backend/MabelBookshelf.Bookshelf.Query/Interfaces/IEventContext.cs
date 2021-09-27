using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Query.Interfaces
{
    public interface IEventContext
    {
        public Task AppendEvent(string streamName, CancellationToken token = default, params DomainEvent[] events);
    }
}