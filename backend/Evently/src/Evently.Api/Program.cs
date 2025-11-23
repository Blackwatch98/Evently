using Evently.Api.Extensions;
using Evently.Application.Events.CreateEvent;
using Evently.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocumentation(builder.Environment);

var app = builder.Build();


app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

//app.UseAuthorization();
//app.MapControllers();

app.MapPost("/api/events", async (CreateEventCommand command, CreateEventHandler handler) =>
{
    var id = await handler.HandleAsync(command);
    return Results.Created($"/api/events/{id}", new { id });
});

app.Run();
