using MShop.Business.Entity;
using MShop.Core.Data;
using MShop.Core.Paginated;

namespace MShop.Repository.Interface
{
    public interface IProductRepository : IRepository<Product>, IPaginated<Product>
    {
        Task<List<Product>> GetProductsPromotions();

        Task<List<Product>> GetProductsByCategoryId(Guid categoryId);

        Task<Product> GetProductWithCategory(Guid id);
    }
}
