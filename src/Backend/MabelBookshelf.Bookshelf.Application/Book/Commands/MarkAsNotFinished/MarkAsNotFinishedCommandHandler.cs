using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkAsNotFinishedCommandHandler : IRequestHandler<MarkAsNotFinishedCommand, bool>
    {
        private readonly IBookRepository _repository;

        public MarkAsNotFinishedCommandHandler(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(MarkAsNotFinishedCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetAsync(request.BookId, cancellationToken);

            if (book == null)
                throw new ArgumentException("Book not found");

            book.MarkAsNotFinished();

            await _repository.UpdateAsync(book, cancellationToken);
            return true;
        }
    }
}