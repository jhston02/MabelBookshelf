using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace MabelBookshelf.BackgroundWorkers
{
    public class ProjectionManagerService : IHostedService
    {
        private readonly  IEnumerable<IProjectionService> _services;
        private readonly CatchUpSubscriptionEventStoreContext _context;
        private readonly List<Action> unsubscribeActions;
        public ProjectionManagerService(IEnumerable<IProjectionService> services, CatchUpSubscriptionEventStoreContext context)
        {
            _services = services;
            _context = context;
            unsubscribeActions = new List<Action>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var service in _services)
            {
                var action = await _context.Subscribe(
                    service.Project,
                    service.GetCurrentPosition,
                    service.Checkpoint,
                    service.CheckpointInterval
                );
                unsubscribeActions.Add(action);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var action in unsubscribeActions)
            {
                action();
            }
            
            return Task.CompletedTask;
        }
    }
}