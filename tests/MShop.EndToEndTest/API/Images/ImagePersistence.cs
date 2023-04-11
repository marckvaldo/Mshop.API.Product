using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.EndToEndTest.API.Images
{
    public class ImagePersistence
    {
        private readonly RepositoryDbContext _context;
        public ImagePersistence(RepositoryDbContext context) 
        {
            _context = context;
        }

        public async Task<BusinessEntity.Image?> GetById(Guid id)
        {
            return await _context.
                Images.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BusinessEntity.Image?>> List()
        {
            return await _context.Images.ToListAsync();
        }

        public async void Create(BusinessEntity.Image request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task CreateList(List<BusinessEntity.Image> request)
        {
            await _context.AddRangeAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
