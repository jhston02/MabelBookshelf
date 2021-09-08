using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class ReadToPageCommandHandler : IRequestHandler<ReadToPageCommand, bool>
    {
        private IBookRepository _repository;

        public ReadToPageCommandHandler(IBookRepository repository)
        {
            this._repository = repository;
        }
        public async Task<bool> Handle(ReadToPageCommand request, CancellationToken cancellationToken)
        {
            var book = _repository.GetAsync(request.BookId) ?? throw new ArgumentException("Book does not exist");
            book.Result.ReadToPage(request.PageNumber);
            await _repository.UpdateAsync(book.Result);
            await _repository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}