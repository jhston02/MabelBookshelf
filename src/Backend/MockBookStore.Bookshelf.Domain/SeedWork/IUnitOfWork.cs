using System.Threading.Tasks;

namespace MockBookStore.Bookshelf.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}