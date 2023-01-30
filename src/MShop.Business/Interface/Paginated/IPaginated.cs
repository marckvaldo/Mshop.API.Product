using MShop.Business.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Paginated
{
    public interface IPaginated<TEntity> where TEntity : Entity.Entity
    {
        Task<PaginatedOutPut<TEntity>> FilterPaginated(PaginatedInPut input); 
    }
}
