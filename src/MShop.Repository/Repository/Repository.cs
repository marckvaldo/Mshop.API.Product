using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Repository.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly RepositoryDbContext _db;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(RepositoryDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public async Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
            //NotFoundException.ThrowIfnull(result, "your search returned null");
            return result;
        }

        public virtual async Task<TEntity?> GetById(Guid Id)
        {
            var result = await _dbSet.FindAsync(Id);
            //NotFoundException.ThrowIfnull(result, "your search returned null");
            return result;
        }

        public virtual async Task<List<TEntity>> GetValuesList()
        {
            var result =  await _dbSet.ToListAsync();
            //NotFoundException.ThrowIfnull(result, "your search returned null");
            return result;
        }

        public virtual async Task<TEntity> GetLastRegister(Expression<Func<TEntity, bool>> predicate)
        {
            var result =  await _dbSet.AsNoTracking().Where(predicate).OrderByDescending(x=>x.Id).FirstAsync();
            //NotFoundException.ThrowIfnull(result, "your search returned null");
            return result;
        }

        public virtual async Task Create(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task DeleteById(TEntity entity)
        {
            _dbSet.Remove(entity);
            await SaveChanges();
        }

        public virtual async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChanges();
        }
        public async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();
        }
        public void Dispose()
        {
            _db?.Dispose();
        }


    }
}
