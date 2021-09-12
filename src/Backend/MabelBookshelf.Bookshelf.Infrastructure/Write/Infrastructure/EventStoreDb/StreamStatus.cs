namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb
{
    public class StreamStatus
    {
        public StreamStatus(string id, string group, string streamName, Status status)
        {
            Id = id;
            Group = group;
            StreamName = streamName;
            Status = status;
        }

        public string Id { get; }
        public string Group { get; }
        public string StreamName { get; }
        public Status Status { get; }
    }

    public enum Status
    {
        Ok,
        Dropped
    }
}