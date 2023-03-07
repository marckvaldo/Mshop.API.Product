using MShop.Business.Entity;
using MShop.Business.Interface.Repository;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Repository.Repository
{
    public class ImagesRepository : Repository<Image>, IImageRepository
    {
        public ImagesRepository(RepositoryDbContext db) : base(db)
        {

        }

        public async Task CreateRange(List<Image> images)
        {
            await _dbSet.AddRangeAsync(images);
            await SaveChanges();
        }
    }
}
