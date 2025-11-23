namespace Evently.Domain.Abstractions
{
    public interface IDomainEvent
    {
        DateTime OccuredAt { get; set; }
    }
}
