using MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Repository
{
    public  interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryProducts(Guid id);

       
    }
}
