using System;
using MabelBookshelf.Bookshelf.Query.Interfaces;
using MabelBookshelf.Bookshelf.Query.MongoDb.Configuration;
using MabelBookshelf.Bookshelf.Query.MongoDb.Projections;
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
                new BookshelfPreviewConfiguration("test", 1, "bookshelf_preview"));
            return projectionService;
        }
    }
}