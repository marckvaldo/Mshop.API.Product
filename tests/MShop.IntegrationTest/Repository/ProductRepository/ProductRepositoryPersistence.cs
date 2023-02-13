using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.IntegrationTests.Repository
{
    public class ProductRepositoryPersistence
    {
        private readonly RepositoryDbContext _context;

        public ProductRepositoryPersistence(RepositoryDbContext context)
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
