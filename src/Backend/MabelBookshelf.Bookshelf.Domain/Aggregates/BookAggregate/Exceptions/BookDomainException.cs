#nullable enable
using System;
using System.Runtime.Serialization;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    [Serializable]
    public class BookDomainException : Exception
    {
        public BookDomainException()
        {
        }

        protected BookDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BookDomainException(string? message) : base(message)
        {
        }

        public BookDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}