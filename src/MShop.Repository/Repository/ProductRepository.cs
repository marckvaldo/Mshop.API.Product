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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(RepositoryDbContext db) : base(db)
        {

        }

        public async Task<Product> getProductWithCategory(Guid id)
        {
          return await  _db.Products.Where(p => p.Id == id).Include(c => c.CategoryId).FirstAsync();
        }

    }
}
