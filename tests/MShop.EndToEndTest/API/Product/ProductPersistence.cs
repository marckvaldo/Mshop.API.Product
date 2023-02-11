using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.EndToEndTest.API.Product
{
    public class ProductPersistence
    {
        private readonly RepositoryDbContext _context;

        public ProductPersistence(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessEntity.Product?> GetById(Guid id)
        {
            return await _context.
                Products.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BusinessEntity.Product?>> List()
        {
            return await _context.Products.ToListAsync();
        }

        public async void Create(BusinessEntity.Product request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();  
        }

        public async void CreateList(List<BusinessEntity.Product> request)
        {
            await _context.AddRangeAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
