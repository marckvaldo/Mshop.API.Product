using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using MShop.Business.Enum.Paginated;
using MShop.Business.Exception;
using MShop.Business.Interface.Repository;
using MShop.Business.Paginated;
using MShop.Repository.Context;


namespace MShop.Repository.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(RepositoryDbContext db) : base(db)
        {

        }

        public async Task<Product> GetProductWithCategory(Guid id)
        {
            return await _dbSet.Where(p => p.Id == id)
                .Include(c => c.Category)
                .FirstOrDefaultAsync();
        }

        public async Task<PaginatedOutPut<Product>> FilterPaginated(PaginatedInPut input)
        {
            var toSkip = (input.Page - 1) * input.PerPage;
            var query = _dbSet.AsNoTracking();

            query = AddOrderToQuery(query, input.OrderBy, input.Order);

            if (!string.IsNullOrWhiteSpace(input.Search))
                query.Where(p => p.Name.Contains(input.Search));

            var total = await query.CountAsync();
            var product = await query.Skip(toSkip).Take(input.PerPage)
                         .Include(c => c.Category).ToListAsync();

            //NotFoundException.ThrowIfnull(product);
            return new PaginatedOutPut<Product>(input.Page, input.PerPage, total, product);

        }

        public async Task<List<Product>> GetProductsPromotions()
        {
            var result = await _dbSet.Where(c => c.IsSale == true).Include(c => c.Category).ToListAsync();
            return result;
        }

        private IQueryable<Product> AddOrderToQuery(IQueryable<Product> query, string orderBay, SearchOrder order)
        {
            if (string.IsNullOrWhiteSpace(orderBay))
                return query;
                

            return (orderBay.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => query.OrderBy(x=>x.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(x=>x.Id),
                ("price", SearchOrder.Asc) => query.OrderBy(x=>x.Price),
                ("price", SearchOrder.Desc) => query.OrderByDescending(x=>x.Price),
                _ => query.OrderBy(x => x.Name)
            };
        }


        public async Task<List<Product>> GetProductsByCategoryId(Guid categoryId)
        {
            return await _dbSet.Where(c=>c.CategoryId == categoryId).ToListAsync();
        }
    }
}
