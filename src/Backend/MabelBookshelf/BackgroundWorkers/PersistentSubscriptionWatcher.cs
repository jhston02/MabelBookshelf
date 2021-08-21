using System;
using System.Collections;
using System.Collections.Generic;
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
        private PersistentSubscriptionWatcherConfiguration configuration;
        private PersistentSubscriptionEventStoreContext ctx;
        private IServiceProvider services;
        
        public PersistentSubscriptionWatcher(IServiceProvider services, PersistentSubscriptionWatcherConfiguration configuration, PersistentSubscriptionEventStoreContext ctx)
        {
            this.services = services;
            this.configuration = configuration;
            this.ctx = ctx;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sleepTimespan = new TimeSpan(0, 0, 10);
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var groupStream in configuration.GroupStreams)
                {
                    var status = ctx.GetStreamStatus(groupStream.group, groupStream.stream);
                    if(ctx.GetStreamStatus(groupStream.group, groupStream.stream) == null || status.Status == Status.Dropped)
                    {
                        await ctx.Subscribe<Guid>(groupStream.group, groupStream.stream,
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
                            stoppingToken);
                    }
                }

                await Task.Delay(sleepTimespan, stoppingToken);
            }
        }
    }

    public class PersistentSubscriptionWatcherConfiguration
    {
        public IEnumerable<(string group, string stream)> GroupStreams { get; set; }
    }
}