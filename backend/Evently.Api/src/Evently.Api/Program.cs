using Evently.Api.Endpoints.Auth;
using Evently.Api.Endpoints.Events;
using Evently.Api.Endpoints.Outbox;
using Evently.Api.Endpoints.Registrations;
using Evently.Api.Endpoints.Users;
using Evently.Api.Extensions;
using Evently.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocumentation(builder.Environment);

var app = builder.Build();


app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.MapEventEndpoints();
app.MapRegistrationEndpoints();
app.MapOutboxEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();
