namespace MabelBookshelf.Bookshelf.Query.MongoDb.BookshelfUpcaster.Configuration
{
    public class BookshelfUpcasterConfiguration : MongoProjectionConfiguration
    {
        public BookshelfUpcasterConfiguration(string databaseName, int version, string collectionName) : base(databaseName, version)
        {
            CollectionName = collectionName;
        }
        
        public string CollectionName { get; }
    }
}