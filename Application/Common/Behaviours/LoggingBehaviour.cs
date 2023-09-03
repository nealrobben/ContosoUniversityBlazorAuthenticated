namespace ContosoUniversityBlazor.Application.Common.Behaviours;

using ContosoUniversityBlazor.Application.Common.Interfaces;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? string.Empty;

        _logger.LogInformation("ContosoUniversityBlazor Request: {Name} {@UserId} {@Request}",
            requestName, userId, request);

        return Task.CompletedTask;
    }
}
