using FluentValidation;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Infrastructure.Behaviors;
using MabelBookshelf.Bookshelf.Application.Infrastructure.ExternalBookServices;
using MabelBookshelf.Bookshelf.Domain.SeedWork;
using MabelBookshelf.Bookshelf.Domain.Shared;
using MabelBookshelf.Bookshelf.Infrastructure.Bookshelf;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MabelBookshelf.Infrastructure
{
    internal static class ApplicationExtensions
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(CreateBookshelfCommand), typeof(Entity<>),
                typeof(EventStoreDbBookshelfRepository));
            AssemblyScanner.FindValidatorsInAssembly(typeof(CreateBookshelfCommand).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }

        public static void AddExternalBookService(this IServiceCollection services)
        {
            services.AddHttpClient<GoogleApiExternalBookService>();
            services.AddSingleton<IExternalBookService, GoogleApiExternalBookService>();
            services.Decorate<IExternalBookService, CachingExternalBookServiceDecorator>();
        }
    }
}