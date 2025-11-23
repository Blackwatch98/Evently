using Evently.Application.Abstractions;
using Evently.Application.Events.CreateEvent;
using Evently.Domain.Abstractions;
using Evently.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddDataAccess(builder.Configuration);

            services.AddHttpContextAccessor();

            RegisterRepositories(services);
            RegisterServices(services);
            return builder;
        }

        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventlyDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<CreateEventHandler>();
        }
    }
}
