using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkAsNotFinishedCommandValidator : AbstractValidator<MarkAsNotFinishedCommand>
    {
        public MarkAsNotFinishedCommandValidator(IBookRepository repository)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.BookId).CustomAsync(async (x, context, _) =>
            {
                try
                {
                    if (!await repository.Exists(x))
                    {
                        context.AddFailure("Book doesn't exist");
                    }
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
        }
    }