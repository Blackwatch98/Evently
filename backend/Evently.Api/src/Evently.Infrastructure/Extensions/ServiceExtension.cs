using Evently.Application.Abstractions;
using Evently.Application.Features.Auth;
using Evently.Application.Features.Events.CreateEvent;
using Evently.Application.Features.Registrations.RegisterForEvent;
using Evently.Domain.Aggregates.EventAggregate.DomainEvents;
using Evently.Domain.Aggregates.RegistrationAggregate.DomainEvents;
using Evently.Infrastructure.DomainEvents;
using Evently.Infrastructure.Messaging;
using Evently.Infrastructure.Outbox;
using Evently.Infrastructure.Projections.Events;
using Evently.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Infrastructure.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EventlyDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"), b =>
                    b.MigrationsAssembly("Evently.Infrastructure")));

        return services;
    }

    public static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<LogoutCommandHandler>();
        services.AddScoped<RefreshTokenCommandHandler>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventHandler<EventCreated>, LogEventCreatedHandler>();
        services.AddScoped<IDomainEventHandler<EventCreated>, CreateEventReadModelHandler>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<IDomainEventHandler<EventCreated>, PublishEventCreatedToOutboxHandler>();
        services.AddScoped<IDomainEventHandler<RegistrationCreated>, RegistrationCreatedToOutboxHandler>();
    }

    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
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
