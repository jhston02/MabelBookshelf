#nullable enable
using System;
using System.Runtime.Serialization;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate
{
    [Serializable]
    public class BookshelfDomainException : Exception
    {
        public BookshelfDomainException()
        {
        }

        protected BookshelfDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BookshelfDomainException(string? message) : base(message)
        {
        }

        public BookshelfDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}