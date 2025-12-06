using Evently.Application.Abstractions;
using Evently.Domain.Abstractions;
using Evently.Domain.EventAggregate;
using Evently.Domain.RegistrationAggregate;

namespace Evently.Application.Registrations.RegisterForEvent
{
    public sealed class RegisterForEventHandler
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterForEventHandler(
            IRegistrationRepository registrationRepository,
            IEventRepository eventRepository,
            IUnitOfWork unitOfWork)
        {
            _registrationRepository = registrationRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> HandleAsync(RegisterForEventCommand command, CancellationToken cancellationToken = default)
        {
            Event? evt = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken);
            if (evt is null)
            {
                throw new DomainException("Event not found.");
            }

            bool alreadyRegistered = await _registrationRepository.ExistsAsync(
                command.EventId,
                command.Email,
                cancellationToken);

            if (alreadyRegistered)
            {
                throw new DomainException("You are already registered for this event.");
            }

            int currentCount = await _registrationRepository.CountForEventAsync(
                command.EventId,
                cancellationToken);

            if (currentCount >= evt.Capacity)
            {
                throw new DomainException("Event is full.");
            }

            var registration = Registration.Create(command.EventId, command.Email);

            await _registrationRepository.AddAsync(registration, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return registration.IdRegistration;
        }
    }
}
