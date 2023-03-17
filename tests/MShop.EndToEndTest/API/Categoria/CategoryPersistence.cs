using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.EndToEndTest.API.Categoria
{
    public class CategoryPersistence
    {
        private readonly RepositoryDbContext _context;

        public CategoryPersistence(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessEntity.Category?> GetById(Guid id)
        {
            return await _context.
                Categories.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BusinessEntity.Category?> GetByIdName(string name)
        {
            return await _context.
                Categories.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<BusinessEntity.Category?>> List()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task Create(BusinessEntity.Category request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task CreateList(List<BusinessEntity.Category> request)
        {
            await _context.AddRangeAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
