using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class DeleteBookshelfCommand : IRequest<bool>
    {
        public string Id { get; private set; }
        public string OwnerId { get; private set; }

        public DeleteBookshelfCommand(string id, string ownerId)
        {
            this.Id = id;
            this.OwnerId = ownerId;
        }
    }
}