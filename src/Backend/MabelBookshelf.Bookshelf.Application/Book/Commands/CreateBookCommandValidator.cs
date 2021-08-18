using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator(IBookRepository repository, IExternalBookService bookService)
        {
            //TODO: Read layer check if isbn + owner already a combo
            RuleFor(x => x.ExternalId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    await bookService.GetBook(x);
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
    }
}