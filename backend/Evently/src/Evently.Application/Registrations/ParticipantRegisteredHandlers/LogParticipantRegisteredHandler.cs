using Evently.Application.Abstractions;
using Evently.Application.Events.EventCreatedHandlers;
using Evently.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Evently.Application.Registrations.ParticipantRegisteredHandlers
{
    public sealed class LogParticipantRegisteredHandler : IDomainEventHandler<ParticipantRegistered>
    {
        private readonly ILogger<LogParticipantRegisteredHandler> _logger;
        public LogParticipantRegisteredHandler(ILogger<LogParticipantRegisteredHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ParticipantRegistered domainEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Participant registered: {EventId} - {RegistrationId} with email {Email}",
                domainEvent.EventId,
                domainEvent.RegistrationId,
                domainEvent.Email);

            return Task.CompletedTask;
        }
    }
}
