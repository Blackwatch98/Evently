using Evently.Analytics.Configs;
using Evently.Analytics.Infrastructure;
using Evently.Analytics.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB (SQL Server)
builder.Services.AddDbContext<AnalyticsDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AnalyticsDb")));

// Rabbit
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

// Worker (consumer)
builder.Services.AddHostedService<AnalyticsConsumer>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// API endpoints
app.MapGet("/api/analytics/events", async (AnalyticsDbContext db, CancellationToken ct) =>
{
    var items = await db.EventStats
        .OrderByDescending(x => x.RegistrationsCount)
        .ThenBy(x => x.ScheduledAt)
        .Take(50)
        .ToListAsync(ct);

    return Results.Ok(items);
});

app.MapGet("/api/analytics/daily", async (DateTime? from, DateTime? to, AnalyticsDbContext db, CancellationToken ct) =>
{
    var q = db.DailyStats.AsQueryable();
    if (from is not null) q = q.Where(x => x.Date >= from.Value.Date);
    if (to is not null) q = q.Where(x => x.Date <= to.Value.Date);

    var items = await q.OrderBy(x => x.Date).ToListAsync(ct);
    return Results.Ok(items);
});

app.Run();