using Evently.Application.Abstractions;
using Evently.Domain.Abstractions;
using Evently.Domain.RegistrationAggregate;

namespace Evently.Application.Registrations.RegisterForEvent
{
    public sealed class RegisterForEventHandler
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterForEventHandler(
            IRegistrationRepository registrationRepository,
            IUnitOfWork unitOfWork)
        {
            _registrationRepository = registrationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> HandleAsync(RegisterForEventCommand command, CancellationToken cancellationToken = default)
        {
            // TODO (później): sprawdzić czy event istnieje / są miejsca itd.

            var registration = Registration.Create(command.EventId, command.Email);

            await _registrationRepository.AddAsync(registration, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return registration.IdRegistration;
        }
    }
}
