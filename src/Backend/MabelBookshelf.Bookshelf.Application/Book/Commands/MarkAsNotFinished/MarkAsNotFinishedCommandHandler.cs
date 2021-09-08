using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkAsNotFinishedCommandHandler : IRequestHandler<MarkAsNotFinishedCommand, bool>
    {
        private IBookRepository _repository;

        public MarkAsNotFinishedCommandHandler(IBookRepository repository)
        {
            this._repository = repository;
        }
        
        public async Task<bool> Handle(MarkAsNotFinishedCommand request, CancellationToken cancellationToken)
        {
            var book = _repository.GetAsync(request.BookId);
            book.Result.MarkAsNotFinished();
            await _repository.UpdateAsync(book.Result);
            await _repository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}