using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Core.Enum.Paginated;
using MShop.Core.Exception;
using MShop.Core.Paginated;
using MShop.Repository.Context;
using MShop.Repository.Interface;

namespace MShop.Repository.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryDbContext db) : base(db)
        {

        }

        public async Task<Category> GetCategoryProducts(Guid id)
        {
            return await _dbSet.Where(c => c.Id == id).Include(c => c.Products).FirstOrDefaultAsync();
        }

        public async Task<PaginatedOutPut<Category>> FilterPaginated(PaginatedInPut input)
        {
            var toSkip = (input.Page - 1) * input.PerPage;
            var query = _dbSet.AsNoTracking();

            query = AddOrderToQuery(query, input.OrderBy, input.Order);

            if (!string.IsNullOrWhiteSpace(input.Search))
                query.Where(p => p.Name.Contains(input.Search));

            var total = await query.CountAsync();
            var category = await query.Skip(toSkip).Take(input.PerPage)
                         .ToListAsync();

            NotFoundException.ThrowIfnull(category);
            return new PaginatedOutPut<Category>(input.Page, input.PerPage, total, category);
        }

        private IQueryable<Category> AddOrderToQuery(IQueryable<Category> query, string orderBay, SearchOrder order)
        {
            if (string.IsNullOrWhiteSpace(orderBay))
                return query;


            return (orderBay.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
                _ => query.OrderBy(x => x.Name)
            };
        }

        public async Task<Category> GetByName(string name)
        {
            return await _dbSet.Where(c=>c.Name == name).FirstOrDefaultAsync(); 
        }
    }
}
