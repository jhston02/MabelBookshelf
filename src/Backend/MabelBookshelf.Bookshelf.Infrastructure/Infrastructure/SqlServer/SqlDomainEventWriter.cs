using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class SqlDomainEventWriter : IDomainEventWriter
    {
        private string connectionString;
        public SqlDomainEventWriter()
        {
        }


        public Task Write(DomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}