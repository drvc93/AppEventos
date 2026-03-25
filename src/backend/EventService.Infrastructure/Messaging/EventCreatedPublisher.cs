using EventService.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace EventService.Infrastructure.Messaging;

public class EventCreatedPublisher : IEventMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<EventCreatedPublisher> _logger;

    public EventCreatedPublisher(IPublishEndpoint publishEndpoint, ILogger<EventCreatedPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishEventCreatedAsync(EventService.Domain.Entities.Event @event, CancellationToken ct = default)
    {
        var message = new EventCreatedMessage
        {
            MessageId = Guid.NewGuid(),
            EventId = @event.Id,
            Name = @event.Name,
            OccurredAt = DateTime.UtcNow,
            CorrelationId = Guid.NewGuid(),
            Version = 1
        };

        await _publishEndpoint.Publish(message, ct);

        _logger.LogInformation(
            "Published EventCreatedMessage for event {EventId} with MessageId {MessageId}",
            @event.Id, message.MessageId);
    }
}
