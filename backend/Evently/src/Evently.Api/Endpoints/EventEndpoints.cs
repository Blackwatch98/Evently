using Evently.Application.Events.CreateEvent;

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

            return endpoints;
        }
    }
}
