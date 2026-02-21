namespace Evently.Domain.Common
{
    public interface IDomainEvent
    {
        DateTime OccuredAt { get; set; }
    }
}
