using Evently.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Endpoints.Outbox;

public static class OutboxEndpoints
{
    public static IEndpointRouteBuilder MapOutboxEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/outbox");

        group.MapGet("/", async (EventlyDbContext db, CancellationToken ct) =>
        {
            var events = await db.OutboxMessages
                .ToListAsync(ct);

            return Results.Ok(events);
        })
        .WithName("GetOutboxMessages")
        .WithTags("OutboxMessages");

        return endpoints;
    }
}
