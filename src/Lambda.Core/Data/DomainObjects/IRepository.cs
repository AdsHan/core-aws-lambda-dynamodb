namespace Lambda.Core.Data.DomainObjects;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task UpdateAsync(T obj);
    Task AddAsync(T obj);
    Task DeleteAsync(Guid id);
}
