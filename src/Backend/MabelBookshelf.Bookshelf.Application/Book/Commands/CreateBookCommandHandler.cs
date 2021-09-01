using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    using Book=Domain.Aggregates.BookAggregate.Book;
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, bool>
    {
        private IExternalBookService bookService;
        private IBookRepository _bookRepository;
        public CreateBookCommandHandler(IExternalBookService bookService, IBookRepository repository)
        {
            this.bookService = bookService;
            this._bookRepository = repository;
        }
        
        public async Task<bool> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var externalBook = await bookService.GetBook(request.ExternalId);
            var book = new Book(request.Id, externalBook.Title, externalBook.Authors, externalBook.Isbn,
                externalBook.Id, externalBook.TotalPages, request.OwnerId, externalBook.Categories);

            await this._bookRepository.AddAsync(book);
            await this._bookRepository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}