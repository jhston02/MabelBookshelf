using System.Linq;
using EventStore.Client;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(_ =>
            {
                var settings = EventStoreClientSettings
                    .Create(connectionString);
                return new EventStoreClient(settings);
            });

            services.AddScoped<IEventStoreContext, EventStoreContext>();
            services.Decorate<IEventStoreContext, CachingEventStoreContextDecorator>();
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services,
            BookshelfPreviewConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap<BookshelfPreview>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<BookPreview>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            services.AddSingleton<MongoClient>();
            services.AddSingleton(x => configuration);
            return services;
        }

        public static IServiceCollection AddTypeCache(this IServiceCollection services)
        {
            services.AddSingleton<ITypeCache>(_ =>
            {
                var types = typeof(BookCreatedDomainEvent).Assembly.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(DomainEvent)));
                return new DictionaryTypeCache(types.ToDictionary(x => x.Name, x => x));
            });
            return services;
        }
    }
}