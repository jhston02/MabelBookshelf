using FluentValidation;
using MabelBookshelf.Bookshelf.Application.Interfaces;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class ReadToPageCommandValidator : AbstractValidator<ReadToPageCommand>
    {
        public ReadToPageCommandValidator(IExternalBookService bookService)
        {
            RuleFor(x => x.BookId).NotNull();
            RuleFor(x => x.PageNumber).NotNull();
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        }
    }
}