using Evently.Application.Abstractions;
using Evently.Domain.EventAggregate.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Evently.Application.Features.Events.CreateEvent;

public sealed class LogEventCreatedHandler : IDomainEventHandler<EventCreated>
{
    private readonly ILogger<LogEventCreatedHandler> _logger;

    public LogEventCreatedHandler(ILogger<LogEventCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EventCreated domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Event created: {EventId} - {Title} at {ScheduledAt}",
            domainEvent.EventId,
            domainEvent.Title,
            domainEvent.ScheduledAt);

        return Task.CompletedTask;
    }
}
