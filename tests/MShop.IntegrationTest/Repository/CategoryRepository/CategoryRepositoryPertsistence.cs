using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.CategoryRepository
{
    public class CategoryRepositoryPertsistence
    {
        private readonly RepositoryDbContext _context;
        public CategoryRepositoryPertsistence(RepositoryDbContext context) 
        {
            _context = context; 
        }

        public async Task<Category?> GetCategory(Guid id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllCategories() 
        {
            return await _context.Categories.ToListAsync();
        }

        public async void Create(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async void CreateList(IEnumerable<Category> category)
        {
            await _context.AddRangeAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
