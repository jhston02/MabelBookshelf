using System.Threading.Tasks;

namespace MockBookStore.Catalog.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}