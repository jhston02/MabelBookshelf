using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class FinishBookCommandValidator : AbstractValidator<FinishBookCommand>
    {
        public FinishBookCommandValidator(IBookRepository repository)
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Id).CustomAsync(async (x, context, token) =>
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