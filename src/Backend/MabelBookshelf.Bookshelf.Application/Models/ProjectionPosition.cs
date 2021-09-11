namespace MabelBookshelf.Bookshelf.Application.Models
{
    public class ProjectionPosition
    {
        public ProjectionPosition(ulong commitPosition, ulong preparePosition)
        {
            CommitPosition = commitPosition;
            PreparePosition = preparePosition;
        }

        public ulong PreparePosition { get; }
        public ulong CommitPosition { get; }
    }
}