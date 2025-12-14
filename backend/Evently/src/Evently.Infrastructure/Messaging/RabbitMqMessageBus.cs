using Evently.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Evently.Infrastructure.Messaging
{
    public sealed class RabbitMqMessageBus : IMessageBus
    {
        private readonly ILogger<RabbitMqMessageBus> _logger;
        private readonly RabbitMqConfiguration _config;
        private readonly ConnectionFactory _factory;

        public RabbitMqMessageBus(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageBus> logger)
        {
            _config = options.Value;
            _logger = logger;
            _factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.Username,
                Password = _config.Password
            };
        }

        public async Task PublishAsync(string type, string payload, CancellationToken cancellationToken = default)
        {
            // 1. Konwersja payloadu do bajtów
            var body = Encoding.UTF8.GetBytes(payload);

            // 2. Tworzymy połączenie i kanał tylko na czas publikacji
            using var connection = await _factory.CreateConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync();

            // 3. Deklarujemy kolejkę (idempotentne – jak istnieje, to nic się nie stanie)
            await channel.QueueDeclareAsync(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            // 4. Publikujemy wiadomość (tu dopasuj do dokładnej sygnatury swojej biblioteki)
            await channel.BasicPublishAsync(
                exchange: "",                        // domyślny exchange
                routingKey: _config.QueueName,
                mandatory: false,
                body: body,
                cancellationToken: cancellationToken
            );

            _logger.LogInformation(
                "Published integration event to RabbitMQ. Type={Type}, Queue={Queue}",
                type,
                _config.QueueName);
        }
    }
}
