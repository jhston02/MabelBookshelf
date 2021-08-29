using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandValidator : AbstractValidator<AddBookToBookshelfCommand>
    {
        public AddBookToBookshelfCommandValidator()
        {
            //TODO: Add validation
        }
    }
}