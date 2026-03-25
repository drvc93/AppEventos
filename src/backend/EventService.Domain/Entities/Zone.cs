namespace EventService.Domain.Entities;

public class Zone
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Capacity { get; private set; }

    public Event Event { get; private set; } = null!;

    private Zone() { }

    public Zone(string name, decimal price, int capacity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Capacity = capacity;
    }
}
