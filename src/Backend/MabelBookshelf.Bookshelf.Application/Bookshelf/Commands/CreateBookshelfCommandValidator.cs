using FluentValidation;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class CreateBookshelfCommandValidator : AbstractValidator<CreateBookshelfCommand>
    {
        public CreateBookshelfCommandValidator()
        {
            RuleFor(x => x.Name).Custom((x, context) =>
            {
                var filter = new ProfanityFilter.ProfanityFilter();
                if (filter.IsProfanity(x))
                {
                    context.AddFailure("Name must not contain profanity");
                }
            });
        }
    }
}