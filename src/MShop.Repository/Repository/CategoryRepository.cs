using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Business.Interface.Repository;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Repository.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryDbContext db) : base(db)
        {

        }

        public async Task<Category> GetCategoryProducts(Guid id)
        {
            return await _dbSet.Where(c => c.Id == id).Include(c => c.Products).FirstAsync();
        }

        public async Task<bool> GetThereAreProduct(Guid id)
        {
            return _dbSet.Where(c => c.Id == id).Include(c => c.Products).Count() > 0;
        }
    }
}
