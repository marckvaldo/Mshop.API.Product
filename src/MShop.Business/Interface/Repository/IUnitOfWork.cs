namespace MShop.Business.Interface.Repository
{
    public interface IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken = default);

        public Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
