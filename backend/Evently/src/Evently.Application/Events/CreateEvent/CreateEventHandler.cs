using Evently.Application.Abstractions;
using Evently.Domain.EventAggregate;

namespace Evently.Application.Events.CreateEvent;

public sealed class CreateEventHandler
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventHandler(
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(CreateEventCommand command, CancellationToken cancellationToken = default)
    {
        var evt = Event.Create(
            command.Title,
            command.Description,
            command.ScheduledAt,
            command.Capacity);

        await _eventRepository.AddAsync(evt, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return evt.IdEvent;
    }
}
