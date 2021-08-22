using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using IdentityServer4.Events;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MabelBookshelf.BackgroundWorkers
{
    public class PersistentSubscriptionWatcher : BackgroundService
    {
        private PersistantSubscriptionSettings configuration;
        private PersistentSubscriptionEventStoreContext ctx;
        private IServiceProvider services;

        public PersistentSubscriptionWatcher(IServiceProvider services,
            PersistantSubscriptionSettings configuration, PersistentSubscriptionEventStoreContext ctx)
        {
            this.services = services;
            this.configuration = configuration;
            this.ctx = ctx;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sleepTimespan = new TimeSpan(0, 0, 10);

            foreach (var config in configuration.Connections)
            {
                for (int i = 0; i < config.Count; i++)
                {
                    await Subscribe(config.GroupName, config.StreamName, stoppingToken);
                }
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var droppedStreams = ctx.GetDroppedStreams();
                foreach (var stream in droppedStreams)
                {
                    await Subscribe(stream.Group, stream.StreamName, stoppingToken, stream.Id);
                }
                await Task.Delay(sleepTimespan, stoppingToken);
            }
        }

        private async Task Subscribe(string groupName, string streamName, CancellationToken stoppingToken, string oldStream = null)
        {
            await ctx.Subscribe<Guid>(groupName, streamName,
                async (x) =>
                {
                    using (var scope = services.CreateScope())
                    {
                        var mediator =
                            scope.ServiceProvider
                                .GetRequiredService<IMediator>();
                        await mediator.Publish(x, stoppingToken);
                    }
                },
                stoppingToken, oldStream);
        }
    }

    public class PersistantSubscriptionSettings
    {
        public List<Connection> Connections { get; set; }
    }

    public class Connection
    {
        public string GroupName { get; set; }
        public string StreamName { get; set; }
        public int Count { get; set; }
    }
}