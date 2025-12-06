using Evently.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Evently.Infrastructure.Messaging
{
    public sealed class LoggingMessageBus : IMessageBus
    {
        private readonly ILogger<LoggingMessageBus> _logger;

        public LoggingMessageBus(ILogger<LoggingMessageBus> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(string type, string payload, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Publishing integration event {Type}: {Payload}", type, payload);
            return Task.CompletedTask;
        }
    }
}
