using Evently.Api.Endpoints.Registrations.Contracts;
using Evently.Application.Registrations.RegisterForEvent;

namespace Evently.Api.Endpoints.Registrations
{
    public static class RegistrationEndpoints
    {
        public static IEndpointRouteBuilder MapRegistrationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/events");

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

            return endpoints;
        }
    }
}
