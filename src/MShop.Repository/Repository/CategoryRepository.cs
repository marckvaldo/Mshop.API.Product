using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Business.Enum.Paginated;
using MShop.Business.Exception;
using MShop.Business.Interface.Repository;
using MShop.Business.Paginated;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
