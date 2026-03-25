namespace EventService.Application.DTOs;

public record CreateEventRequest
{
    public string Name { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string Location { get; init; } = string.Empty;
    public List<CreateZoneRequest> Zones { get; init; } = new();
}

public record CreateZoneRequest
{
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Capacity { get; init; }
}

public record EventResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string Location { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public List<ZoneResponse> Zones { get; init; } = new();
}

public record ZoneResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Capacity { get; init; }
}
