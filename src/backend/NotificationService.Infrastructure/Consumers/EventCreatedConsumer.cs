using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;
using Shared.Contracts;

namespace NotificationService.Infrastructure.Consumers;

public class EventCreatedConsumer : IConsumer<EventCreatedMessage>
{
    private readonly NotificationDbContext _context;
    private readonly ILogger<EventCreatedConsumer> _logger;

    public EventCreatedConsumer(NotificationDbContext context, ILogger<EventCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EventCreatedMessage> context)
    {
        var message = context.Message;

        var alreadyProcessed = await _context.ProcessedMessages
            .AnyAsync(p => p.MessageId == message.MessageId);

        if (alreadyProcessed)
        {
            _logger.LogWarning("Message {MessageId} already processed. Skipping.", message.MessageId);
            return;
        }

        _logger.LogInformation(
            "Sending notification for event '{EventName}' (EventId: {EventId}). " +
            "CorrelationId: {CorrelationId}",
            message.Name, message.EventId, message.CorrelationId);

        var notification = new Notification
        {
            EventId = message.EventId,
            EventName = message.Name,
            Channel = "Email",
            Status = "Sent"
        };
        _context.Notifications.Add(notification);

        _context.ProcessedMessages.Add(new ProcessedMessage
        {
            MessageId = message.MessageId,
            ProcessedAt = DateTime.UtcNow,
            Consumer = nameof(EventCreatedConsumer)
        });

        await _context.SaveChangesAsync();

        _logger.LogInformation("Notification sent and message {MessageId} marked as processed.", message.MessageId);
    }
}
