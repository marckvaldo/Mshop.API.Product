using MShop.Application.Common;
using MShop.Application.UseCases.Category.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Paginated;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public class ListCategory : BaseUseCase, IListCategory
    {
        private readonly ICategoryRepository _categoryRepositiry;
        private readonly List<CategoryModelOutPut> _listCategory;
        public ListCategory(ICategoryRepository categoryRepositiry, INotification notification) : base(notification)
        {
            _categoryRepositiry = categoryRepositiry;
            _listCategory = new List<CategoryModelOutPut>();
        }

        public async Task<ListCategoryOutPut> Handler(ListCategoryInPut request)
        {
            var paginate = new PaginatedInPut(
                request.Page, 
                request.PerPage, 
                request.Search, 
                request.Sort,
                request.Dir);

            var categorys = await _categoryRepositiry.FilterPaginated(paginate);

            return new ListCategoryOutPut(
                categorys.CurrentPage,
                categorys.PerPage,
                categorys.Total,
                categorys.Itens.Select(x => new CategoryModelOutPut(
                    x.Id, 
                    x.Name, 
                    x.IsActive
                    )).ToList()); 
        }
    }
}
