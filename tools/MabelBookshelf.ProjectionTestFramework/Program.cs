using System;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview;
using MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Projections;
using MongoDB.Driver;

namespace MabelBookshelf.ProjectionTestFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var test = new ProjectionTester(10000);
            var projection = GetProjectionService();
            var results = test.TestProjection(projection).Result;
            foreach (var result in results)
                Console.WriteLine(
                    $"Entity {result.domainEvent}, Finished in {result.time}, Throughput {result.entityCount / result.time.Seconds} per second");

            Console.ReadLine();
        }

        private static IProjectionService GetProjectionService()
        {
            var projectionService = new MongoBookshelfPreviewProjection(new MongoClient("mongodb://localhost:27017"),
                new BookshelfPreviewConfiguration
                    { DatabaseName = "test", Version = 1, CollectionName = "bookshelf_preview" });
            return projectionService;
        }
    }
}