using EventService.Domain.Entities;

namespace EventService.Domain.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Event>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Event entity, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
