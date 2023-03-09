using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Business.Interface.Repository;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
            _dbSet.AddRange(images);
            await SaveChanges();
        }

        public async Task DeleteByIdProduct(Guid productId)
        {
            var images =  _dbSet.Where(x=>x.ProductId == productId).ToList();
            foreach (Image image in images)
             _dbSet.Remove(image);

            await SaveChanges();
        }

    }
}
