using System;
using System.Linq;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb;
using MabelBookshelf.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MabelBookshelf", Version = "v1" });
            });

            ConfigureBookshelfDomainServices(services);
        }

        private void ConfigureBookshelfDomainServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddEventStore(Configuration);
            services.AddMediatR();
            services.AddRepositories();
            services.AddExternalBookService();
            services.AddSingleton<ProfanityFilter.ProfanityFilter>();
            services.AddTypeCache();
            services.AddPersistentSubscriptions(Configuration);
            services.AddCatchupSubscriptions();
            services.AddProjectionsAndQueries(Configuration);
            services.AddSingleton<CatchUpSubscriptionEventStoreContext>();
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (x, y) => false;
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

            options.Map<ArgumentException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                return factory.CreateProblemDetails(ctx, 400, detail: ex.Message, title: "Bad request");
            });
            options.Map<BookshelfDomainException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                return factory.CreateProblemDetails(ctx, 400, detail: ex.Message, title: "Bad request");
            });
            options.Map<BookDomainException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                return factory.CreateProblemDetails(ctx, 400, detail: ex.Message, title: "Bad request");
            });
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