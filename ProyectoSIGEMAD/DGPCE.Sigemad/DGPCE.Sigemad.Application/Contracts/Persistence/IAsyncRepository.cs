using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllNoTrackingAsync();

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   string includeString = null,
                                   bool disableTracking = true);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   List<Expression<Func<T, object>>> includes = null,
                                   bool disableTracking = true);

    Task<IReadOnlyList<TResult>> GetAsync<TResult>(
                                    Expression<Func<T, TResult>> selector,
                                    Expression<Func<T, bool>> predicate = null,
                                    Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null,
                                    List<Expression<Func<T, object>>> includes = null,
                                    bool disableTracking = true,
                                    bool distinct = false);


    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByIdAsync(int id);

    Task<T> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(T entity);


    void AddEntity(T entity);

    void UpdateEntity(T entity);

    void DeleteEntity(T entity);

    Task<T> GetByIdWithSpec(ISpecification<T> spec);

    Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);

    Task<int> CountAsync(ISpecification<T> spec);

    Task<T?> GetFirstOrDefaultAsync(ISpecification<T> spec);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
