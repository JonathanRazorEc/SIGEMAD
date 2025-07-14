namespace DGPCE.Sigemad.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {

        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task<int> Complete();
    }
}
