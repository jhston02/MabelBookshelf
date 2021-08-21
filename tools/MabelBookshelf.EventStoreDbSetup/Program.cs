using System;
using CommandLine;
using EventStore.Client;
using EventStore.ClientAPI;
using PersistentSubscriptionSettings = EventStore.Client.PersistentSubscriptionSettings;

namespace MabelBookshelf.EventStoreDbSetup
{
    class Program
    {
        public class Options
        {
            [Option('c', "connection", Required = true, HelpText = "Connection string to evenstore db")]
            public string ConnectionString { get; set; }
        }
        
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    //TODO: Creds when we need real deployments
                    var settings = EventStoreClientSettings
                        .Create(o.ConnectionString);
                    var managementClient = new EventStoreProjectionManagementClient(settings);
                    //managementClient.EnableAsync("$by_category").GetAwaiter().GetResult();
                    
                    using var client = new EventStorePersistentSubscriptionsClient(
                        EventStoreClientSettings.Create(o.ConnectionString)
                    );
                    var pSettings = new PersistentSubscriptionSettings(namedConsumerStrategy: "Pinned", resolveLinkTos: true);
                    client.CreateAsync(
                        "$ce-book",
                        "readmodelbook-group",
                        pSettings).GetAwaiter().GetResult();
                    
                    client.CreateAsync(
                        "$ce-bookshelf",
                        "readmodelbookshelf-group",
                        pSettings).GetAwaiter().GetResult();
                })
                .WithNotParsed(o => Console.WriteLine("Not valid connections string"));
        }
    }
}