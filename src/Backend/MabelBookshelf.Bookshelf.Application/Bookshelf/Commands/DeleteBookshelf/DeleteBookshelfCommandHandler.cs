using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    using Bookshelf=Domain.Aggregates.BookshelfAggregate.Bookshelf;
    
    public class DeleteBookshelfCommandHandler : IRequestHandler<DeleteBookshelfCommand, bool>
    {
        private readonly IBookshelfRepository _bookshelfRepository;
        
        public DeleteBookshelfCommandHandler(IBookshelfRepository bookshelfRepository)
        {
            this._bookshelfRepository = bookshelfRepository;
        }
        
        public async Task<bool> Handle(DeleteBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf = await _bookshelfRepository.GetAsync(request.BookshelfId);
            bookshelf.Delete();
            await _bookshelfRepository.UpdateAsync(bookshelf);
            return true;
        }
    }
}