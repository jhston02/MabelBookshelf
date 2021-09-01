using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    using Bookshelf=Domain.Aggregates.BookshelfAggregate.Bookshelf;
    public class CreateBookshelfCommandHandler : IRequestHandler<CreateBookshelfCommand, bool>
    {
        private IBookshelfRepository _repository;
        
        public CreateBookshelfCommandHandler(IBookshelfRepository repository)
        {
            this._repository = repository;
        }
        
        public async Task<bool> Handle(CreateBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf = new Bookshelf(request.Id, request.Name, request.OwnerId);
            await _repository.AddAsync(bookshelf);
            await _repository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}