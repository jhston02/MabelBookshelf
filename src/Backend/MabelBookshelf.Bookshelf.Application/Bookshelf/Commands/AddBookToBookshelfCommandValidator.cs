using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandValidator : AbstractValidator<AddBookToBookshelfCommand>
    {
        public AddBookToBookshelfCommandValidator(IBookRepository repository)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.ShelfId).NotNull();
            RuleFor(x => x.BookId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    if (!await repository.Exists(x, token)) context.AddFailure("Book doesn't exist");
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
    }
}