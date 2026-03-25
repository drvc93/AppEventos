using EventService.Application.DTOs;
using EventService.Application.Interfaces;
using EventService.Domain.Interfaces;
using MediatR;

namespace EventService.Application.Queries;

public record GetEventsQuery : IRequest<IReadOnlyList<EventResponse>>;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, IReadOnlyList<EventResponse>>
{
    private readonly IEventRepository _repository;
    private readonly ICacheService _cache;
    private const string CacheKey = "events:all";

    public GetEventsQueryHandler(IEventRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EventResponse>> Handle(GetEventsQuery request, CancellationToken ct)
    {
        var cached = await _cache.GetAsync<List<EventResponse>>(CacheKey, ct);
        if (cached is not null)
            return cached;

        var events = await _repository.GetAllAsync(ct);

        var response = events.Select(e => new EventResponse
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
        }).ToList();

        await _cache.SetAsync(CacheKey, response, TimeSpan.FromMinutes(5), ct);

        return response;
    }
}
