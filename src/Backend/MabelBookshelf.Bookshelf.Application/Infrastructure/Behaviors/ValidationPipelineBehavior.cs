using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Infrastructure.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this._validators = validators;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationFailures = _validators.Select(async x => await x.ValidateAsync(request, cancellationToken))
                .SelectMany(x => x.Result.Errors)
                .Where(x => x != null)
                .ToList();

            if (validationFailures.Any())
            {
                throw new ArgumentException(string.Join("\r\n", validationFailures));
            }

            return await next();
        }
    }
}