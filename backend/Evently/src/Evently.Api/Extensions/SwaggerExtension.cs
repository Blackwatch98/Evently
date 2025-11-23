using Microsoft.OpenApi;
using System.Reflection;

namespace Evently.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(
            this IServiceCollection services,
            IWebHostEnvironment env)
        {
            // Jeśli chcesz, możesz wyrzucić AddEndpointsApiExplorer z Program.cs,
            // bo robimy to tutaj.
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Evently API",
                    Version = "v1",
                    Description = $"API do zarządzania wydarzeniami. ENV = {env.EnvironmentName}"
                });

                // XML comments - zadziała tylko jeśli włączysz generowanie .xml w csproj,
                // ale to nie psuje nic, jeśli pliku nie ma.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                // 🔥 NA RAZIE ZERO JWT, ZERO OpenApiReference – tylko to, co potrzebne,
                // żeby wygenerować poprawne openapi: 3.x.
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            // UFAJEMY domyślnej konfiguracji Swashbuckle – BEZ ręcznego endpointa
            app.UseSwaggerUI();

            return app;
        }
    }
}
