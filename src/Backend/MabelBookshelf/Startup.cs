using System;
using System.Linq;
using EventStore.Client;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using MabelBookshelf.BackgroundWorkers;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Infrastructure.Behaviors;
using MabelBookshelf.Bookshelf.Application.Infrastructure.ExternalBookServices;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Infrastructure.Book;
using MabelBookshelf.Bookshelf.Infrastructure.Bookshelf;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;
using MabelBookshelf.Bookshelf.Infrastructure.Interfaces;
using MabelBookshelf.Bookshelf.Infrastructure.Projections.BookshelfPreviewProjections;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace MabelBookshelf
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProblemDetails(ConfigureProblemDetails)
                .AddControllers()
                .AddProblemDetailsConventions();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MabelBookshelf", Version = "v1" });
            });

            ConfigureBookshelfDomainServices(services);
        }

        private void ConfigureBookshelfDomainServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton(_ =>
            {
                var settings = EventStoreClientSettings
                    .Create(Configuration.GetConnectionString("EventStoreDbConnectionString"));
                return new EventStoreClient(settings);
            });

            services.AddMediatR(typeof(Startup), typeof(CreateBookshelfCommand), typeof(Entity<>),
                typeof(EventStoreDbBookshelfRepository));
            AssemblyScanner.FindValidatorsInAssembly(typeof(CreateBookshelfCommand).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped<IBookshelfRepository, EventStoreDbBookshelfRepository>();
            services.AddSingleton<IExternalBookService, GoogleApiExternalBookService>();
            services.AddScoped<IBookRepository, EventStoreDbBookRepository>();
            services.AddScoped<IEventStoreContext, EventStoreContext>();
            services.Decorate<IEventStoreContext, CachingEventStoreContextDecorator>();
            services.Decorate<IExternalBookService, CachingExternalBookServiceDecorator>();
            services.AddSingleton<ProfanityFilter.ProfanityFilter>();
            services.AddHttpClient<GoogleApiExternalBookService>();
            services.AddSingleton<ITypeCache>(_ =>
            {
                var types = typeof(BookCreatedDomainEvent).Assembly.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(DomainEvent)));
                return new DictionaryTypeCache(types.ToDictionary(x => x.Name, x => x));
            });
            services.AddSingleton(_ =>
            {
                var settings = new PersistantSubscriptionSettings();
                Configuration.GetSection("PersistantSubscriptionSettings").Bind(settings);
                return settings;
            });
            services.AddSingleton(_ =>
            {
                var settings = EventStoreClientSettings
                    .Create(Configuration.GetConnectionString("EventStoreDbConnectionString"));
                return new EventStorePersistentSubscriptionsClient(settings);
            });
            services.AddSingleton<PersistentSubscriptionEventStoreContext>();
            services.AddSingleton<CatchUpSubscriptionEventStoreContext>();
            //TODO: Scan assembly for these
            services.AddSingleton<IProjectionService, BookshelfPreviewProjection>();
            services.AddSingleton<MongoClient>();
            services.AddSingleton<BookshelfPreviewProjectionConfiguration>(x =>
            {
                var settings = new BookshelfPreviewProjectionConfiguration();
                Configuration.GetSection("BookshelfPreviewProjectionConfiguration").Bind(settings);
                return settings;
            });
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            // Custom mapping function for FluentValidation's ValidationException.
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(validationFailure => validationFailure.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors);
            });

            options.MapToStatusCode<ArgumentException>(400);
            options.MapToStatusCode<BookshelfDomainException>(400);
            options.MapToStatusCode<BookDomainException>(400);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MabelBookshelf v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}