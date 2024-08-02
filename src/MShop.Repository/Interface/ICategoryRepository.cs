using MShop.Business.Entity;
using MShop.Core.Data;
using MShop.Core.Paginated;

namespace MShop.Repository.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryProducts(Guid id);

        Task<PaginatedOutPut<Category>> FilterPaginated(PaginatedInPut input);

        Task<Category> GetByName(string name);
    }
}
