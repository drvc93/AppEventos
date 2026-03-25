using EventService.Application.DTOs;
using EventService.Domain.Interfaces;
using MediatR;

namespace EventService.Application.Queries;

public record GetEventByIdQuery(Guid Id) : IRequest<EventResponse?>;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventResponse?>
{
    private readonly IEventRepository _repository;

    public GetEventByIdQueryHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventResponse?> Handle(GetEventByIdQuery request, CancellationToken ct)
    {
        var @event = await _repository.GetByIdAsync(request.Id, ct);
        if (@event is null) return null;

        return new EventResponse
        {
            Id = @event.Id,
            Name = @event.Name,
            Date = @event.Date,
            Location = @event.Location,
            Status = @event.Status.ToString(),
            CreatedAt = @event.CreatedAt,
            Zones = @event.Zones.Select(z => new ZoneResponse
            {
                Id = z.Id,
                Name = z.Name,
                Price = z.Price,
                Capacity = z.Capacity
            }).ToList()
        };
    }
}
