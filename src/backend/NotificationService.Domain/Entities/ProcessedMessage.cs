namespace NotificationService.Domain.Entities;

public class ProcessedMessage
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Consumer { get; set; } = string.Empty;
}
