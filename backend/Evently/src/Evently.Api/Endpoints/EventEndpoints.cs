using Evently.Api.Contracts.Events;
using Evently.Application.Events.CreateEvent;
using Evently.Application.Registrations.RegisterForEvent;
using Evently.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static IEndpointRouteBuilder MapEventEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/events");

            // POST /api/events
            group.MapPost("/", async (CreateEventCommand command, CreateEventHandler handler) =>
            {
                var id = await handler.HandleAsync(command);
                return Results.Created($"/api/events/{id}", new { id });
            })
            .WithName("CreateEvent")
            .WithTags("Events");

            // POST /api/events/{eventId}/registrations
            group.MapPost("/{eventId:guid}/registrations", async (
                Guid eventId,
                RegisterForEventRequest request,
                RegisterForEventHandler handler,
                CancellationToken ct) =>
            {
                var command = new RegisterForEventCommand(eventId, request.Email);
                var registrationId = await handler.HandleAsync(command, ct);

                return Results.Created(
                    $"/api/events/{eventId}/registrations/{registrationId}",
                    new { id = registrationId });
            })
            .WithName("RegisterForEvent")
            .WithTags("Registrations");

            // GET /api/events
            group.MapGet("/", async (EventlyDbContext db, CancellationToken ct) =>
            {
                var events = await db.EventReadModels
                    .OrderBy(e => e.ScheduledAt)
                    .ToListAsync(ct);

                return Results.Ok(events);
            })
            .WithName("GetEvents")
            .WithTags("Events");

            return endpoints;
        }
    }
}
