using Evently.Application.Abstractions;
using Evently.Application.Features.Events.CreateEvent;
using Evently.Application.Features.Registrations.RegisterForEvent;
using Evently.Domain.Aggregates.EventAggregate.DomainEvents;
using Evently.Domain.Aggregates.RegistrationAggregate.DomainEvents;
using Evently.Infrastructure.DomainEvents;
using Evently.Infrastructure.Messaging;
using Evently.Infrastructure.Outbox;
using Evently.Infrastructure.Projections.Events;
using Evently.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Infrastructure.Extensions;

public static class ServiceExtension
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddDataAccess(builder.Configuration);

        services.AddHttpContextAccessor();

        RegisterRepositories(services);
        RegisterServices(services, builder.Configuration);
        return builder;
    }

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EventlyDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"), b =>
                    b.MigrationsAssembly("Evently.Infrastructure")));

        return services;
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventHandler<EventCreated>, LogEventCreatedHandler>();
        services.AddScoped<IDomainEventHandler<EventCreated>, CreateEventReadModelHandler>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<IDomainEventHandler<EventCreated>, PublishEventCreatedToOutboxHandler>();
        services.AddScoped<IDomainEventHandler<RegistrationCreated>, RegistrationCreatedToOutboxHandler>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CreateEventHandler>();
        services.AddScoped<RegisterForEventHandler>();
        //services.AddScoped<IMessageBus, LoggingMessageBus>();
        services.AddHostedService<OutboxProcessor>();

        services.Configure<RabbitMqConfiguration>(
            configuration.GetSection("RabbitMq"));

        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
    }
}
