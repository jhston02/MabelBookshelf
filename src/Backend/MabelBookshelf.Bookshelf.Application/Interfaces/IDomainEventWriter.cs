using System.ComponentModel;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Interfaces
{
    public interface IDomainEventWriter
    {
        Task Write(DomainEvent domainEvent);
    }
}