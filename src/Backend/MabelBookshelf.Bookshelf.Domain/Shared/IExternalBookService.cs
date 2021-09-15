using System.Threading;
using System.Threading.Tasks;

namespace MabelBookshelf.Bookshelf.Domain.Shared
{
    public interface IExternalBookService
    {
        Task<ExternalBook> GetBookAsync(string externalBookId, CancellationToken token = default);
    }
}