using MShop.Application.UseCases.Category.Common;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Core.Paginated;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public class ListCategory : Core.Base.BaseUseCase, IListCategory
    {
        private readonly ICategoryRepository _categoryRepositiry;
        private readonly List<CategoryModelOutPut> _listCategory;
        public ListCategory(ICategoryRepository categoryRepositiry, INotification notification) : base(notification)
        {
            _categoryRepositiry = categoryRepositiry;
            _listCategory = new List<CategoryModelOutPut>();
        }

        public async Task<Result<ListCategoryOutPut>> Handle(ListCategoryInPut request, CancellationToken cancellation)
        {
            var paginate = new PaginatedInPut(
                request.Page, 
                request.PerPage, 
                request.Search, 
                request.Sort,
                request.Dir);

            var categorys = await _categoryRepositiry.FilterPaginated(paginate);

            var listCategory = new ListCategoryOutPut(
                categorys.CurrentPage,
                categorys.PerPage,
                categorys.Total,
                categorys.Itens.Select(x => new CategoryModelOutPut(
                    x.Id, 
                    x.Name, 
                    x.IsActive
                    )).ToList());

            return Result<ListCategoryOutPut>.Success(listCategory);
        }
    }
}
