using System.Threading.Tasks;

namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}