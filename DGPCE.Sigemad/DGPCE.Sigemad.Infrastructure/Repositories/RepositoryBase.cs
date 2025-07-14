using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Infrastructure.Persistence;
using DGPCE.Sigemad.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : class
{
    protected readonly SigemadDbContext _context;

    public RepositoryBase(SigemadDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   string includeString = null,
                                   bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();


        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                 List<Expression<Func<T, object>>> includes = null,
                                 bool disableTracking = true)
    {

        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();


        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true,
            bool distinct = false)
    {
        IQueryable<T> query = _context.Set<T>();

        if (disableTracking)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
            query = query.Where(predicate);

        IQueryable<TResult> selectedQuery = query.Select(selector); // ✅ Aplicamos Select()

        if (distinct)
            selectedQuery = selectedQuery.Distinct(); // ✅ Aplicamos Distinct() si es necesario

        if (orderBy != null)
            return await orderBy(selectedQuery).ToListAsync();

        return await selectedQuery.ToListAsync();

    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void AddEntity(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void UpdateEntity(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteEntity(T entity)
    {
        _context.Set<T>().Remove(entity);
    }


    public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
    {
        var entity = await ApplySpecification(spec).FirstOrDefaultAsync();

        if (spec is BaseSpecification<T> baseSpec && entity != null)
        {
            foreach (var postFilter in baseSpec.PostFilters)
            {
                entity = postFilter(entity);
            }
        }

        return entity;
    }

    public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec)
            .AsNoTracking()
            .CountAsync();
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllNoTrackingAsync()
    {
        return await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<T?> GetFirstOrDefaultAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
    }
}
