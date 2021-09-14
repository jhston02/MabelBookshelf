using System;
using FluentValidation;
using MabelBookshelf.Bookshelf.Application.Exceptions;
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
                    var book = await bookRepository.GetAsync(x, token);
                    if (book == null) context.AddFailure("Book doesn't exist");
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
            //We have caching but this is a little expensive (required a lookup only needed for this check
            //We could always materialize a view or something but it might be out of sync. Regardless it's probably fine
            //No sense in optimizing prematurely just something to keep in mind
            RuleFor(x => x.OwnerId).CustomAsync(async (x, context, token) =>
            {
                try
                {
                    var book = await bookRepository.GetAsync(context.InstanceToValidate.BookId, token);
                    var bookshelf =
                        await bookshelfRepository.GetAsync(context.InstanceToValidate.ShelfId, token: token);
                    if (bookshelf != null && book != null && (book.OwnerId != x || bookshelf.OwnerId != x))
                        throw new UnauthorizedException();
                }
                catch (ArgumentException e)
                {
                    context.AddFailure(e.Message);
                }
            });
        }
    }
}