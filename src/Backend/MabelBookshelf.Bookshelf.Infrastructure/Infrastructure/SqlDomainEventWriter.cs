using System;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class SqlDomainEventWriter : IDomainEventWriter<BookCreatedDomainEvent,Guid>
    {
        private string connectionString;
        public SqlDomainEventWriter()
        {
            this.connectionString = connectionString;
        }
        
        public Task Handle(BookCreatedDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}