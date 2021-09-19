using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkBookAsWantedCommandHandler : IRequestHandler<MarkBookAsWantedCommand, bool>
    {
        private readonly IBookRepository _repository;

        public MarkBookAsWantedCommandHandler(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(MarkBookAsWantedCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetAsync(request.Id, cancellationToken);

            if (book == null)
                throw new ArgumentException("Book not found");

            book.MarkBookAsWanted();

            await _repository.UpdateAsync(book, cancellationToken);
            return true;
        }
    }
}