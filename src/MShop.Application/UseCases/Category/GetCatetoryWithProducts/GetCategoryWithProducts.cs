using MShop.Application.UseCases.Category.GetCatetoryWithProducts;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory
{
    public class GetCategoryWithProducts : Core.Base.BaseUseCase, IGetCategoryWithProducts
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryWithProducts(INotification notification, ICategoryRepository categoryRepository) : base(notification)
          => _categoryRepository = categoryRepository;
        public async Task<Result<GetCategoryWithProductsOutPut>> Handle(GetCategoryWithProductsInPut request, CancellationToken cancellationToken)
        {
            var category = await  _categoryRepository.GetCategoryProducts(request.Id);
            //NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");
            
            if (NotifyErrorIfNull(category, "não foi possivel localizar a categoria da base de dados!"))
                return Result<GetCategoryWithProductsOutPut>.Error();

            if (!category.IsValid(Notifications))
                return Result<GetCategoryWithProductsOutPut>.Error();

            List<ProductModelOutPut> listProdutos = new List<ProductModelOutPut>();
            foreach(var item in category.Products)
            {
                listProdutos.Add(new ProductModelOutPut(
                    item.Id,
                    item.Description,
                    item.Name,
                    item.Price,
                    item.Thumb?.Path,
                    item.Stock,
                    item.IsActive,
                    item.CategoryId
                    ));
            }

            /*return new GetCategoryWithProductsOutPut(
                request.Id, 
                category.Name, 
                category.IsActive,
                listProdutos);*/

            //return GetCategoryWithProductsOutPut.FromCategory(category, listProdutos);
            return Result<GetCategoryWithProductsOutPut>.Success(GetCategoryWithProductsOutPut.FromCategory(category,listProdutos));
        }
    }
}
