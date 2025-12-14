namespace Evently.Infrastructure.Messaging
{
    public sealed class RabbitMqConfiguration
    {
        public required string HostName { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string QueueName { get; set; }
    }
}
