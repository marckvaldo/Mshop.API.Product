using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Paginated;

namespace MShop.Application.UseCases.Product.ListProducts
{
    public class ListProducts : BaseUseCase, IListProducts
    {
        private readonly IProductRepository _productRepostory;

        public ListProducts(IProductRepository productRepostory, INotification notification) : base(notification)
        {
            _productRepostory = productRepostory;   
        }

        public async Task<ListProductsOutPut> Handler(ListProductInPut request)
        {

            var paginatedInPut = new PaginatedInPut(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
                );

            var paginateOutPut = await _productRepostory.FilterPaginated(paginatedInPut);

            return new ListProductsOutPut(
                paginateOutPut.CurrentPage,
                paginateOutPut.PerPage,
                paginateOutPut.Total,
                paginateOutPut.Itens.Select(x => new ProductModelOutPut
                (
                    x.Id,
                    x.Description,
                    x.Name,
                    x.Price,
                    x.Thumb?.Path,
                    x.Stock,
                    x.IsActive,
                    x.CategoryId,
                    (new CategoryModelOutPut(x.Category.Id,x.Category.Name,x.Category.IsActive))
                )).ToList()
                );

            
        }
    }
}
