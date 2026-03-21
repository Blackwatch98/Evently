using Evently.Application.Abstractions;
using Evently.Application.Features.Events.CreateEvent;
using Evently.Domain.Aggregates.EventAggregate;
using NSubstitute;
using Shouldly;

namespace Evently.Application.Tests;

public sealed class CreateEventHandlerTests
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task HandleAsync_Should_Add_Event_To_Repository()
    {
        // Arrange
        var handler = new CreateEventHandler(_eventRepository, _unitOfWork);

        var scheduledAt = new DateTime(2026, 4, 10, 18, 0, 0, DateTimeKind.Utc);
        var command = new CreateEventCommand(
            "DDD Warsaw",
            "Domain-driven design meetup",
            scheduledAt,
            100);

        // Act
        await handler.HandleAsync(command);
        // Assert
        await _eventRepository.Received(1).AddAsync(
            Arg.Is<Event>(e =>
                e.Title == "DDD Warsaw" &&
                e.Description == "Domain-driven design meetup" &&
                e.ScheduledAt == scheduledAt &&
                e.Capacity == 100),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_Should_Save_Changes()
    {
        // Arrange
        var handler = new CreateEventHandler(_eventRepository, _unitOfWork);

        var command = new CreateEventCommand(
            "Architecture Meetup",
            "Clean architecture session",
            new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc),
            50);

        // Act
        await handler.HandleAsync(command);
        // Assert
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Created_Event_Id()
    {
        // Arrange
        var handler = new CreateEventHandler(_eventRepository, _unitOfWork);

        // Act
        var command = new CreateEventCommand(
            "DDD Warsaw",
            "Domain-driven design meetup",
            new DateTime(2026, 4, 10, 18, 0, 0, DateTimeKind.Utc),
            100);

        // Assert
        var result = await handler.HandleAsync(command);

        result.ShouldNotBe(Guid.Empty);
    }
}
