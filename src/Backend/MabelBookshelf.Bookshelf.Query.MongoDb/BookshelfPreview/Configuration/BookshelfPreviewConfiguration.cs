namespace MabelBookshelf.Bookshelf.Query.MongoDb.Configuration
{
    public class BookshelfPreviewConfiguration : MongoProjectionConfiguration
    {
        public BookshelfPreviewConfiguration(string databaseName, int version, string collectionName) : base(databaseName, version)
        {
            CollectionName = collectionName;
        }
        
        public string CollectionName { get; }
    }
}