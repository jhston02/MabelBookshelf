namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure
{
    public class StreamStatus
    {
        public string Id { get; private set; }
        public string Group { get; private set; }
        public string StreamName { get; private set; }
        public Status Status { get; private set; }

        public StreamStatus(string id, string @group, string streamName, Status status)
        {
            Id = id;
            Group = @group;
            StreamName = streamName;
            Status = status;
        }
    }

    public enum Status
    {
        Ok,
        Dropped
    }
}