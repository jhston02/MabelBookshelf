using System.ComponentModel;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Interfaces
{
    public interface IDomainEventWriter<T,TV> where T : DomainEvent<TV>
    {
        Task Handle(T domainEvent);
    }
}