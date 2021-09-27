using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Query.Events
{
    public record BookAddedToBookshelfWithBookInfo(string BookId, string Title, string[] Authors, string ExternalId,
        string OwnerId, string[] Categories, string Status, Guid BookshelfId) : DomainEvent;
}