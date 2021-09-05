using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IExternalBookService bookService;

        public CreateBookCommandHandler(IExternalBookService bookService, IBookRepository repository)
        {
            this.bookService = bookService;
            _bookRepository = repository;
        }

        public async Task<bool> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var externalBook = await bookService.GetBookAsync(request.ExternalId, cancellationToken);
            var book = new Domain.Aggregates.BookAggregate.Book($"{request.OwnerId}-{externalBook.Isbn}",
                externalBook.Title, externalBook.Authors, externalBook.Isbn,
                externalBook.Id, externalBook.TotalPages, request.OwnerId, externalBook.Categories);

            await _bookRepository.AddAsync(book, cancellationToken);
            await _bookRepository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}