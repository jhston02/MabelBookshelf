using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class DeleteBookshelfCommandHandler : IRequestHandler<DeleteBookshelfCommand, bool>
    {
        private readonly IBookshelfRepository _bookshelfRepository;

        public DeleteBookshelfCommandHandler(IBookshelfRepository bookshelfRepository)
        {
            _bookshelfRepository = bookshelfRepository;
        }

        public async Task<bool> Handle(DeleteBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf = await _bookshelfRepository.GetAsync(request.Id, token: cancellationToken);
            bookshelf.Delete();
            await _bookshelfRepository.UpdateAsync(bookshelf, cancellationToken);
            return true;
        }
    }
}