using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandValidator : AbstractValidator<AddBookToBookshelfCommand>
    {
        
        public AddBookToBookshelfCommandValidator(IBookRepository repository)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.ShelfId).NotNull();
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