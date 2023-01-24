﻿using Microsoft.EntityFrameworkCore;
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
            return await _db.Categorys.Where(c => c.Id == id).Include(c => c.products).FirstAsync();
        }
    }
}
