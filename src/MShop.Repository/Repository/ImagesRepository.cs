﻿using MShop.Business.Entity;
using MShop.Repository.Context;
using MShop.Repository.Interface;

namespace MShop.Repository.Repository
{
    public class ImagesRepository : Repository<Image>, IImageRepository
    {
        public ImagesRepository(RepositoryDbContext db) : base(db)
        {

        }

        public async Task CreateRange(List<Image> images, CancellationToken cancellationToken)
        {
            await _dbSet.AddRangeAsync(images,cancellationToken);
            //await SaveChanges();
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
