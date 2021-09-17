using System;
using System.Linq;
using EventStore.Client;
using FluentValidation;
using MabelBookshelf.BackgroundWorkers;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure;
using MabelBookshelf.Bookshelf.Infrastructure.Book;
using MabelBookshelf.Bookshelf.Infrastructure.Bookshelf;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Projections;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Queries;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MabelBookshelf.Infrastructure
{
    internal static class InfrastructureExtensions
    {
        public static void AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("EventStoreDbConnectionString");
            services.AddSingleton(_ =>
            {
                var settings = EventStoreClientSettings
                    .Create(connectionString);
                return new EventStoreClient(settings);
            });
            
            services.AddSingleton(_ =>
            {
                var settings = EventStoreClientSettings
                    .Create(connectionString);
                return new EventStorePersistentSubscriptionsClient(settings);
            });
            
            services.AddScoped<IEventStoreContext, EventStoreContext>();
            services.Decorate<IEventStoreContext, CachingEventStoreContextDecorator>();
        }

        public static void AddPersistentSubscriptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_ =>
            {
                var settings = new PersistantSubscriptionSettings();
                configuration.GetSection("PersistantSubscriptionSettings").Bind(settings);
                return settings;
            });
            
            services.AddSingleton(_ =>
            {
                var settings = EventStoreClientSettings
                    .Create(configuration.GetConnectionString("EventStoreDbConnectionString"));
                return new EventStorePersistentSubscriptionsClient(settings);
            });
        }

        public static void AddProjectionsAndQueries(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .Scan(scan => scan
                .FromAssemblyOf<MongoBookshelfPreviewProjection>()
                .AddClasses(classes => classes.AssignableTo<IProjectionService>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            
            services.SetupMongo(configuration);
            services.SetupQueries();
        }

        private static void SetupMongo(this IServiceCollection services, IConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap<BookshelfPreview>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            services.AddSingleton<MongoClient>();
            services.AddSingleton(x =>
            {
                var settings = new BookshelfPreviewConfiguration();
                configuration.GetSection("BookshelfPreviewProjectionConfiguration").Bind(settings);
                return settings;
            });
        }

        private static void SetupQueries(this IServiceCollection services)
        {
            services.AddScoped<IBookshelfPreviewQueries, MongoBookshelfPreviewQueries>();
        }

        public static void AddCatchupSubscriptions(this IServiceCollection services)
        {
            services.AddSingleton<PersistentSubscriptionEventStoreContext>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookshelfRepository, EventStoreDbBookshelfRepository>();
            services.AddScoped<IBookRepository, EventStoreDbBookRepository>();
        }

        public static void AddTypeCache(this IServiceCollection services)
        {
            services.AddSingleton<ITypeCache>(_ =>
            {
                var types = typeof(BookCreatedDomainEvent).Assembly.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(DomainEvent)));
                return new DictionaryTypeCache(types.ToDictionary(x => x.Name, x => x));
            });
        }
    }
}