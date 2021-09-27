using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Query.Queries.Preview.Models;

namespace MabelBookshelf.Bookshelf.Query.Queries.Preview
{
    public interface IBookshelfPreviewQueries
    {
        public Task<IEnumerable<BookshelfPreview>> Previews(string ownerId, uint skip, uint take,
            CancellationToken token = default);
    }
}