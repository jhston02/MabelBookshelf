﻿using System.Threading;
using FluentValidation;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class DeleteBookshelfCommandValidator : AbstractValidator<DeleteBookshelfCommand>
    {
        public DeleteBookshelfCommandValidator(IBookshelfRepository bookshelfRepository)
        {
            RuleFor(x => x.OwnerId).CustomAsync(async (x, context, token) =>
            {
                var bookshelf = await bookshelfRepository.GetAsync(context.InstanceToValidate.BookshelfId);
                if(x != bookshelf.OwnerId)
                    context.AddFailure("Unauthorized");
            });
        }
    }
}