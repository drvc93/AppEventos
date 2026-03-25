namespace EventService.Domain.Events;

public record EventCreatedDomainEvent(
    Guid EventId,
    string Name,
    DateTime OccurredAt
);
