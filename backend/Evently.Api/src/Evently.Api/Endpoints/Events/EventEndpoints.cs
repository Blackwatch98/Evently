using Evently.Application.Events.CreateEvent;
using Evently.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Endpoints.Events;

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
