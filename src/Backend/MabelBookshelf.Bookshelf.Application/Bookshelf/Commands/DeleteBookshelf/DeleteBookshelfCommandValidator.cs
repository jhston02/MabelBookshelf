using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class DeleteBookshelfCommandValidator : AbstractValidator<DeleteBookshelfCommand>
    {
        public DeleteBookshelfCommandValidator(IBookshelfRepository bookshelfRepository)
        {
            RuleFor(x => x.Id).CustomAsync(async (x, context, _) =>
            {
                var bookshelf = await bookshelfRepository.GetAsync(context.InstanceToValidate.Id);
                if (bookshelf == null)
                    context.AddFailure(nameof(DeleteBookshelfCommand.Id), "Bookshelf does not exist");
            });
        }
    }
}