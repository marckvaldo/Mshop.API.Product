using MShop.Business.Entity;
using MShop.Business.Interface.Paginated;

namespace MShop.Business.Interface.Repository
{
    public interface IProductRepository : IRepository<Product>, IPaginated<Product>
    {
        Task<List<Product>> GetProductsPromotions();

        Task<List<Product>> GetProductsByCategoryId(Guid categoryId);

        Task<Product> GetProductWithCategory(Guid id);
    }
}
