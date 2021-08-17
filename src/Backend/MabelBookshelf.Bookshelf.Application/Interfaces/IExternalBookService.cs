using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Models;

namespace MabelBookshelf.Bookshelf.Application.Interfaces
{
    public interface IExternalBookService
    {
        Task<ExternalBook> GetBook(string externalBookId);
    }
}