using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using MabelBookshelf.Bookshelf.Infrastructure.Models;

namespace MabelBookshelf.ProjectionTestFramework
{
    public class ProjectionTester
    {
        private readonly int _entityCount;
        private readonly DataGenerator _generator;
        private ulong count;

        public ProjectionTester(int entityCount)
        {
            _entityCount = entityCount;
            _generator = new DataGenerator();
        }

        public async Task<List<(string domainEvent, int entityCount, TimeSpan time)>> TestProjection(
            IProjectionService service)
        {
            var result =
                new List<(string domainEvent, int entityCount, TimeSpan time)>();

            var createdResult = await TestBookCreated(service);
            result.Add((nameof(BookCreatedDomainEvent), _entityCount, createdResult));

            var createdSResult = await TestBookshelfCreated(service);
            result.Add((nameof(BookshelfCreatedDomainEvent), _entityCount, createdSResult));

            var addedResult = await TestBookAdded(service);
            result.Add((nameof(AddedBookToBookshelfDomainEvent), _entityCount, addedResult));

            var removedResult = await TestBookRemoved(service);
            result.Add((nameof(RemovedBookFromBookshelfDomainEvent), _entityCount, removedResult));

            var renamedResult = await TestBookshelfRenamed(service);
            result.Add((nameof(RenamedBookshelfDomainEvent), _entityCount, renamedResult));

            var deletedResult = await TestBookshelfDeleted(service);
            result.Add((nameof(BookshelfDeletedDomainEvent), _entityCount, deletedResult));

            return result;
        }

        private async Task<TimeSpan> TestBookCreated(IProjectionService service)
        {
            var domainEvents = _generator.GetBookCreatedDomainEvents(_entityCount).ToList();
            var converted = ConvertToStreamEntry(domainEvents.Cast<DomainEvent>().ToList()).ToList();
            return await Time(converted, service);
        }

        private async Task<TimeSpan> TestBookshelfCreated(IProjectionService service)
        {
            var domainEvents = _generator.GetBookshelfCreatedDomainEvents(_entityCount).ToList();
            var converted = ConvertToStreamEntry(domainEvents.Cast<DomainEvent>().ToList()).ToList();
            return await Time(converted, service);
        }

        private async Task<TimeSpan> TestBookAdded(IProjectionService service)
        {
            var domainEvents = _generator.GetAddedBookToBookshelfDomainEvents(200, _entityCount / 200).ToList();
            var converted = ConvertToStreamEntry(domainEvents.Cast<DomainEvent>().ToList()).ToList();
            return await Time(converted, service);
        }

        private async Task<TimeSpan> TestBookRemoved(IProjectionService service)
        {
            var domainEvents = _generator.GetRemovedBookFromBookshelfDomainEvents().ToList();
            var converted = ConvertToStreamEntry(domainEvents.Cast<DomainEvent>().ToList()).ToList();
            return await Time(converted, service);
        }

        private async Task<TimeSpan> TestBookshelfRenamed(IProjectionService service)
        {
            var domainEvents = _generator.GetBookshelfRenamedDomainEvents().ToList();
            var converted = ConvertToStreamEntry(domainEvents.Cast<DomainEvent>().ToList()).ToList();
            return await Time(converted, service);
        }

        private async Task<TimeSpan> TestBookshelfDeleted(IProjectionService service)
        {
            var domainEvents = _generator.GetBookshelfDeletedDomainEvents().ToList();
            var converted = ConvertToStreamEntry(domainEvents.Cast<DomainEvent>().ToList()).ToList();
            return await Time(converted, service);
        }

        private IEnumerable<StreamEntry> ConvertToStreamEntry(List<DomainEvent> events)
        {
            foreach (var @event in events)
            {
                yield return new StreamEntry(count, @event);
                count++;
            }
        }

        private async Task<TimeSpan> Time(List<StreamEntry> events, IProjectionService service)
        {
            var watch = new Stopwatch();
            watch.Start();
            foreach (var @event in events) await service.ProjectAsync(@event);
            watch.Stop();
            return watch.Elapsed;
        }
    }
}