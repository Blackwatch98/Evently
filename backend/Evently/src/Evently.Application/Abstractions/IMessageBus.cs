namespace Evently.Application.Abstractions
{
    public interface IMessageBus
    {
        Task PublishAsync(string type, string payload, CancellationToken cancellationToken = default);
    }
}
