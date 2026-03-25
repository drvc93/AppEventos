using EventService.Application.DTOs;
using EventService.Application.Interfaces;
using EventService.Domain.Interfaces;
using MediatR;

namespace EventService.Application.Commands;

public record CreateEventCommand(CreateEventRequest Request) : IRequest<EventResponse>;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
    private readonly IEventRepository _repository;
    private readonly IEventMessagePublisher _publisher;
    private readonly ICacheService _cache;

    public CreateEventCommandHandler(
        IEventRepository repository,
        IEventMessagePublisher publisher,
        ICacheService cache)
    {
        _repository = repository;
        _publisher = publisher;
        _cache = cache;
    }

    public async Task<EventResponse> Handle(CreateEventCommand command, CancellationToken ct)
    {
        var request = command.Request;

        var @event = new Domain.Entities.Event(request.Name, request.Date, request.Location);

        foreach (var zone in request.Zones)
        {
            @event.AddZone(zone.Name, zone.Price, zone.Capacity);
        }

        @event.Publish();

        await _repository.AddAsync(@event, ct);
        await _repository.SaveChangesAsync(ct);

        await _publisher.PublishEventCreatedAsync(@event, ct);

        await _cache.RemoveAsync("events:all", ct);

        @event.ClearDomainEvents();

        return MapToResponse(@event);
    }

    private static EventResponse MapToResponse(Domain.Entities.Event e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Date = e.Date,
        Location = e.Location,
        Status = e.Status.ToString(),
        CreatedAt = e.CreatedAt,
        Zones = e.Zones.Select(z => new ZoneResponse
        {
            Id = z.Id,
            Name = z.Name,
            Price = z.Price,
            Capacity = z.Capacity
        }).ToList()
    };
}
