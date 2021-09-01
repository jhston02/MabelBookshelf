using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using MabelBookshelf.Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MabelBookshelf", Version = "v1"});
            });

            ConfigureBookshelfDomainServices(services);
        }

        private void ConfigureBookshelfDomainServices(IServiceCollection services)
        {
            services.AddSingleton((_) =>
            {
                var settings = EventStoreClientSettings
                    .Create(Configuration.GetConnectionString("EventStoreDbConnectionString"));
                return new EventStoreClient(settings);
            });
            
            services.AddMediatR(typeof(Startup), typeof(CreateBookshelfCommand), typeof(Entity), typeof(EventStoreDbBookshelfRepository));
            AssemblyScanner.FindValidatorsInAssembly(typeof(CreateBookshelfCommand).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped<IBookshelfRepository,EventStoreDbBookshelfRepository>();
            services.AddScoped<IExternalBookService, GoogleApiExternalBookService>();
            services.AddScoped<IBookRepository, EventStoreDbBookRepository>();
            services.AddScoped<IEventStoreContext, EventStoreContext>();
            services.Decorate<IEventStoreContext, CachingEventStoreContextDecorator>();
            services.AddSingleton<ProfanityFilter.ProfanityFilter>();
            services.AddHttpClient<GoogleApiExternalBookService>();
            services.AddSingleton<ITypeCache>(_ =>
            {
                var types = typeof(BookCreatedDomainEvent).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DomainEvent)));
                return new DictionaryTypeCache(types.ToDictionary(x => x.Name, x => x));
            });
            services.AddSingleton(x =>
            {
                var settings = new PersistantSubscriptionSettings();
                Configuration.GetSection("PersistantSubscriptionSettings").Bind(settings);
                return settings;
            });
            services.AddSingleton((_) =>
            {
                var settings = EventStoreClientSettings
                    .Create(Configuration.GetConnectionString("EventStoreDbConnectionString"));
                return new EventStorePersistentSubscriptionsClient(settings);
            });
            services.AddSingleton<PersistentSubscriptionEventStoreContext>();
            services.AddScoped(typeof(IDomainEventWriter), typeof(SqlDomainEventWriter));
        }
        
        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            // Custom mapping function for FluentValidation's ValidationException.
            options.MapFluentValidationException();

            // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
            // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
            options.Rethrow<NotSupportedException>();

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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