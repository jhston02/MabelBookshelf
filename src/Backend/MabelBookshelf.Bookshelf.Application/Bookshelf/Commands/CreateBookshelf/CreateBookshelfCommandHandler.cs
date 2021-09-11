using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class CreateBookshelfCommandHandler : IRequestHandler<CreateBookshelfCommand, bool>
    {
        private readonly IBookshelfRepository _repository;

        public CreateBookshelfCommandHandler(IBookshelfRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CreateBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf =
                new Domain.Aggregates.BookshelfAggregate.Bookshelf(request.Id, request.Name, request.OwnerId);
            await _repository.AddAsync(bookshelf, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}