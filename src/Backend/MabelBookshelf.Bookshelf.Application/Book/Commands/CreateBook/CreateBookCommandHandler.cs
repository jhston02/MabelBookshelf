using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using MabelBookshelf.Bookshelf.Domain.Shared;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, string>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IExternalBookService bookService;

        public CreateBookCommandHandler(IExternalBookService bookService, IBookRepository repository)
        {
            this.bookService = bookService;
            _bookRepository = repository;
        }

        public async Task<string> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var volumeInfo = await VolumeInfo.FromExternalId(
                request.ExternalId, bookService,
                cancellationToken);
            var book = new Domain.Aggregates.BookAggregate.Book($"{request.OwnerId}-{volumeInfo.Isbn}", request.OwnerId,
                volumeInfo);

            await _bookRepository.AddAsync(book, cancellationToken);
            await _bookRepository.UnitOfWork.SaveChangesAsync();
            return book.Id;
        }
    }
}