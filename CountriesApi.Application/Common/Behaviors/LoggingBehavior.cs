using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CountriesApi.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            logger.LogInformation(
                "Handling {RequestName}: {@Request}",
                typeof(TRequest).Name,
                request
            );

            var response = await next();

            logger.LogInformation(
                "Handled {RequestName} with {@Response}",
                typeof(TRequest).Name,
                response
            );

            return response;
        }
    }
}
