using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandValidator : AbstractValidator<AddBookToBookshelfCommand>
    {
        public AddBookToBookshelfCommandValidator(IBookRepository bookRepository, IBookshelfRepository bookshelfRepository)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.ShelfId).NotNull();
            RuleFor(x => x.BookId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    if (!await bookRepository.ExistsAsync(x, token)) context.AddFailure("Book doesn't exist");
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
            RuleFor(x => x.ShelfId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    var bookshelf = await bookshelfRepository.GetAsync(x, token:token);
                    if (bookshelf == null) context.AddFailure("Bookshelf doesn't exist");
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
    }
}