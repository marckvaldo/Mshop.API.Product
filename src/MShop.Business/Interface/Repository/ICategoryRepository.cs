using MShop.Business.Entity;
using MShop.Business.Paginated;

namespace MShop.Business.Interface.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryProducts(Guid id);

        Task<PaginatedOutPut<Category>> FilterPaginated(PaginatedInPut input);
    }
}
