using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandValidator : AbstractValidator<AddBookToBookshelfCommand>
    {
        public AddBookToBookshelfCommandValidator(IBookshelfRepository repository)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.ShelfId).NotNull();
        }
    }
}