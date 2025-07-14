using System.Collections;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Infrastructure.Persistence;
using DGPCE.Sigemad.Application.Exceptions;

namespace DGPCE.Sigemad.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly SigemadDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _currentTransaction;
    private Hashtable? _repositories;

    public UnitOfWork(SigemadDbContext context,
                      ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public SigemadDbContext SigemadDbContext => _context;

    // ─────────────────────────────  TRANSACTIONS  ─────────────────────────────
    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is null)
            _currentTransaction = await _context.Database
                                               .BeginTransactionAsync(
                                                    System.Data.IsolationLevel.ReadCommitted);
    }

    public async Task CommitAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    // ───────────────────────────────  SAVE  ───────────────────────────────────
    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.GetBaseException() is SqlException sql)
            {
                // Ej.: 2601 / 2627 (duplicados), 547 (FK) …
                _logger.LogError(sql,
                                 "SQL {Num}: {Msg}",
                                 sql.Number,
                                 sql.Message);

                // Opcional: relanzar una BadRequest legible
                throw new BadRequestException($"SQL {sql.Number}: {sql.Message}", dbEx);
            }

            throw;
        }
    }

    // ────────────────────────────  REPOSITORIES  ─────────────────────────────
    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repoType = typeof(RepositoryBase<>).MakeGenericType(typeof(TEntity));
            var repoInstance = Activator.CreateInstance(repoType, _context);
            _repositories[type] = repoInstance!;
        }

        return (IAsyncRepository<TEntity>)_repositories[type]!;
    }

    // ───────────────────────────────  DISPOSE  ───────────────────────────────
    public void Dispose() => _context.Dispose();
}
