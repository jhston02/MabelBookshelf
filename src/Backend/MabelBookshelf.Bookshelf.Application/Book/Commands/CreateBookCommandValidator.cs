using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Application.Interfaces;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator(IExternalBookService bookService)
        {
            //TODO: Read layer check if isbn + owner already a combo
            RuleFor(x => x.ExternalId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    await bookService.GetBookAsync(x, token);
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
    }
}