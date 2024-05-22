using MShop.Core.DomainObject;
using System.Linq.Expressions;

namespace MShop.Core.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Create(TEntity entity, CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
        Task DeleteById(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity?> GetById(Guid Id);
        Task<List<TEntity>> GetValuesList();
        Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetLastRegister(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
    }
}
