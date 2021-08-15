using System;
using System.Runtime.Serialization;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Exceptions
{
    public class DuplicateBookshelfException : Exception
    {
        protected DuplicateBookshelfException(Guid id, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Id = id;
        }

        public DuplicateBookshelfException(Guid id, string? message) : base(message)
        {
            this.Id = id;
        }

        public DuplicateBookshelfException(Guid id, string? message, Exception? innerException) : base(message, innerException)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
        
        public DuplicateBookshelfException(Guid id) : base()
        {
            this.Id = id;
        }
    }
}