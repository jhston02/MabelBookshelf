namespace MabelBookshelf.Bookshelf.Query.MongoDb
{
    public class MongoProjectionConfiguration
    {
        public MongoProjectionConfiguration(string databaseName, int version)
        {
            DatabaseName = databaseName + $"_v{version}";
        }

        public string DatabaseName { get;  }
    }
}