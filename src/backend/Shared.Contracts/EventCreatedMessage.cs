namespace Shared.Contracts;

public record EventCreatedMessage
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public Guid EventId { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public int Version { get; init; } = 1;
}
