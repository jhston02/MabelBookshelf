using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands.StartReading
{
    public class StartReadingCommandValidator: AbstractValidator<MarkAsNotFinishedCommand>
    {
        public StartReadingCommandValidator(IBookRepository repository)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.BookId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    if (!await repository.ExistsAsync(x, token)) context.AddFailure("Book doesn't exist");
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
    }
}