using MShop.Business.Entity;
using MShop.Business.Interface.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Repository
{
    public interface IProductRepository : IRepository<Product>, IPaginated<Product>
    {
        Task<List<Product>> GetProductsPromotions();

        Task<List<Product>> GetProductsByCategoryId(Guid categoryId);

        Task<Product> GetProductWithCategory(Guid id); 
    }
}
