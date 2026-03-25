using EventService.Domain.Enums;
using EventService.Domain.Events;

namespace EventService.Domain.Entities;

public class Event
{
    private readonly List<Zone> _zones = new();
    private readonly List<EventCreatedDomainEvent> _domainEvents = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public EventStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Zone> Zones => _zones.AsReadOnly();
    public IReadOnlyCollection<EventCreatedDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Event() { }

    public Event(string name, DateTime date, string location)
    {
        Id = Guid.NewGuid();
        Name = name;
        Date = date;
        Location = location;
        Status = EventStatus.Draft;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddZone(string name, decimal price, int capacity)
    {
        var zone = new Zone(name, price, capacity);
        _zones.Add(zone);
    }

    public void Publish()
    {
        Status = EventStatus.Published;
        _domainEvents.Add(new EventCreatedDomainEvent(Id, Name, DateTime.UtcNow));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
