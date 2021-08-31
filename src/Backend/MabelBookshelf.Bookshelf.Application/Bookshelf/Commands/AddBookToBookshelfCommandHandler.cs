using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandHandler : IRequestHandler<AddBookToBookshelfCommand, bool>
    {
        private IBookshelfRepository _repository;

        public AddBookToBookshelfCommandHandler(IBookshelfRepository repository)
        {
            this._repository = repository;
        }

        public async Task<bool> Handle(AddBookToBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf = _repository.Get(request.ShelfId) ?? throw new ArgumentException("Bookshelf does not exist");
            bookshelf.Result.AddBook(request.BookId);
           await _repository.Update(bookshelf.Result);
           await _repository.UnitOfWork.SaveChangesAsync();
           return true;
        }
    }
}
