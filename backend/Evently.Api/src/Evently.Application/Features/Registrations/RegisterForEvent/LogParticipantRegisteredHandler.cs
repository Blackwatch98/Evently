using Evently.Application.Abstractions;
using Evently.Domain.RegistrationAggregate.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Evently.Application.Features.Registrations.RegisterForEvent;

public sealed class LogParticipantRegisteredHandler : IDomainEventHandler<RegistrationCreated>
{
    private readonly ILogger<LogParticipantRegisteredHandler> _logger;
    public LogParticipantRegisteredHandler(ILogger<LogParticipantRegisteredHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(RegistrationCreated domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Participant registered: {EventId} - {RegistrationId} with email {Email}",
            domainEvent.EventId,
            domainEvent.RegistrationId,
            domainEvent.Email);

        return Task.CompletedTask;
    }
}
