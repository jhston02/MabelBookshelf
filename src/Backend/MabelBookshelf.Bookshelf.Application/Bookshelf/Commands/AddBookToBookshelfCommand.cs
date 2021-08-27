using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommand
    {
        public Guid BookId { get; private set; }
        public Guid ShelfId { get; private set; }
        public AddBookToBookshelfCommand(Guid bookId, Guid shelfId)
        {
            this.BookId = bookId;
            this.ShelfId = shelfId
        }
    }
}
