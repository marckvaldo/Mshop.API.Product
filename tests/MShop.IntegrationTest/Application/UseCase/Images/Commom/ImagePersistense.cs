using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Images.Commom
{
    public class ImagePersistense
    {
        private readonly RepositoryDbContext _context;

        public ImagePersistense(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessEntity.Image> GetImage(Guid id)
        {
            return await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<BusinessEntity.Image>> GetAllImage()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task Create(BusinessEntity.Image category)
        {
            await _context.Images.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task CreateList(IEnumerable<BusinessEntity.Image> categories)
        {
            await _context.Images.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }
    }
}
