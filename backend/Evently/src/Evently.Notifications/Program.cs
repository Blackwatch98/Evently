using Evently.Notifications;
using Evently.Notifications.Configs;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var rabbitSection = context.Configuration.GetSection("RabbitMq");
        services.Configure<RabbitMqConfiguration>(rabbitSection);

        services.AddHostedService<EventCreatedConsumer>();

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
    })
    .Build();

await host.RunAsync();
