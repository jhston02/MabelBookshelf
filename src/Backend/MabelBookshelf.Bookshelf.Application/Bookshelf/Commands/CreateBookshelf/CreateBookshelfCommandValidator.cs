using FluentValidation;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class CreateBookshelfCommandValidator : AbstractValidator<CreateBookshelfCommand>
    {
        public CreateBookshelfCommandValidator(ProfanityFilter.ProfanityFilter filter)
        {
            RuleFor(x => x.Name).Custom((x, context) =>
            {
                if (filter.IsProfanity(x))
                {
                    context.AddFailure("Name must not contain profanity");
                }
            });
        }
    }
}