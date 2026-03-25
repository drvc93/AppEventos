using EventService.Domain.Entities;

namespace EventService.Application.Interfaces;

public interface IEventMessagePublisher
{
    Task PublishEventCreatedAsync(Event @event, CancellationToken ct = default);
}
