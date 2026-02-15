using Evently.Api.Endpoints;
using Evently.Api.Extensions;
using Evently.Infrastructure.Extensions;

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

app.Run();
