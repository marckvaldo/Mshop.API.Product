using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.ImagenRepository
{
    public class ImageRepositoryPersistence
    {
        private readonly RepositoryDbContext _context;

        public ImageRepositoryPersistence(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<Image?> GetImage(Guid id)
        {
            return await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Image>> GetAllImage()
        {
            return await _context.Images.ToListAsync();
        }

        public async void Create(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async void CreateList(IEnumerable<Image> images)
        {
            await _context.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }
    }
}
