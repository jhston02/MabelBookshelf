using System.IO;
using System.Reflection;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Projections;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MabelBookshelf.BackgroundTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                #if Debug
                .AddJsonFile("appsettings.Development.json")
                #endif
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ProjectionManagerService>();
                    ConfigureServices(services, configuration);
                });
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("EventStoreDbConnectionString");
            services.AddEventStore(connectionString);

            services.AddTypeCache();

            services
                .Scan(scan => scan
                    .FromAssemblyOf<MongoBookshelfPreviewProjection>()
                    .AddClasses(classes => classes.AssignableTo<IProjectionService>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());

            services.AddSingleton<CatchUpSubscriptionEventStoreContext>();


            var settings = new BookshelfPreviewConfiguration();
            configuration.GetSection("BookshelfPreviewProjectionConfiguration").Bind(settings);
            services.AddMongo(settings);
        }
    }
}