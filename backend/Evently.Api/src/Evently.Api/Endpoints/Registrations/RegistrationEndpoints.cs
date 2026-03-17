using Evently.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Endpoints.Registrations;

public static class RegistrationEndpoints
{
    public static IEndpointRouteBuilder MapRegistrationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/registrations")
            .WithTags("Registrations");

        // GET /api/registrations
        group.MapGet("/", async (EventlyDbContext db, CancellationToken ct) =>
        {
            var events = await db.Registrations
                .ToListAsync(ct);

            return Results.Ok(events);
        })
        .WithName("GetRegistrations");

        return endpoints;
    }
}
