using MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity.Entity
    {
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task DeleteById(TEntity entity);
        Task<TEntity> GetById(Guid Id);
        Task<List<TEntity>> GetValuesList();
        Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetLastRegister(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
    }
}
