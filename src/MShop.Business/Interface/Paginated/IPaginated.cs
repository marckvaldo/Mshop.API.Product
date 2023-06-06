using MShop.Business.Paginated;

namespace MShop.Business.Interface.Paginated
{
    public interface IPaginated<TEntity> where TEntity : SeedWork.Entity
    {
        Task<PaginatedOutPut<TEntity>> FilterPaginated(PaginatedInPut input);
    }
}
