namespace Evently.Application.IntegrationEvents
{
    public abstract record IntegrationEvent(Guid Id, DateTime OccurredOn);
}
