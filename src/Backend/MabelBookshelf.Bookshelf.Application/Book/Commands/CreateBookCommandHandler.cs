﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    using Book=Domain.Aggregates.BookAggregate.Book;
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, bool>
    {
        private IExternalBookService bookService;
        public CreateBookCommandHandler(IExternalBookService bookService)
        {
            this.bookService = bookService;
        }
        
        public async Task<bool> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var externalBook = await bookService.GetBook(request.ExternalId);
            var book = new Book(request.Id, externalBook.Title, externalBook.Authors, externalBook.Isbn,
                externalBook.Id, externalBook.TotalPages, request.OwnerId, externalBook.Categories);

            throw new NotImplementedException();
        }
    }
}