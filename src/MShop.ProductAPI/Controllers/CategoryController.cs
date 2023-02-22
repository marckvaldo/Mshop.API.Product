using Microsoft.AspNetCore.Mvc;
using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Application.UseCases.Category.DeleteCategory;
using MShop.Application.UseCases.Category.GetCatetory;
using MShop.Application.UseCases.Category.ListCategorys;
using MShop.Application.UseCases.Category.UpdateCategory;
using MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory;
using MShop.Business.Exceptions;
using MShop.Business.Interface;


namespace MShop.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : MainController
    {
        private readonly IGetCategory _getCategory;
        private readonly IListCategory _listCategory;
        private readonly ICreateCategory _createCategory;
        private readonly IUpdateCategory _updateCategory;
        private readonly IDeleteCategory _deleteCategory;
        private readonly IGetCategoryWithProducts _getCatetoryWithProducts;
        public CategoryController(
            IGetCategory getCategory,
            IListCategory listCategory,
            ICreateCategory createCategory,
            IUpdateCategory updateCategory,
            IDeleteCategory deleteCategory,
            IGetCategoryWithProducts getCatetoryWithProducts,
            INotification notification) : base(notification)
        {
            _getCategory = getCategory;
            _listCategory = listCategory;
            _createCategory = createCategory;
            _updateCategory = updateCategory;
            _deleteCategory = deleteCategory;  
            _getCatetoryWithProducts = getCatetoryWithProducts; 
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CategoryModelOutPut>> Category(Guid id)
        {
            try
            {
                return CustomResponse(await _getCategory.Handler(id));
            }
            catch (Exception erro)
            {
                Notify(erro.Message);
                return CustomResponse();
            }

        }

        [HttpGet("list-category")]
        public async Task<ActionResult<List<CategoryModelOutPut>>> ListCategory()
        {
            return CustomResponse(await _listCategory.Handler());   
        }


        [HttpGet("list-category-products/{id:Guid}")]
        public async Task<ActionResult<List<GetCategoryWithProductsOutPut>>> ListCategoryProdutcs(Guid Id)
        {
            return CustomResponse(await _getCatetoryWithProducts.Handler(Id));
        }


        [HttpPost]
        public async Task<ActionResult<CategoryModelOutPut>> Create(CreateCategoryInPut request)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _createCategory.Handler(request));
        }

        [HttpPut("{Id:Guid}")]
        public async Task<ActionResult<CategoryModelOutPut>> Update(Guid Id, UpdateCategoryInPut request)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (Id != request.Id)
            {
                Notify("O id informado não é o mesmo passado como parametro");
                return CustomResponse(request);
            }
            return CustomResponse(await _updateCategory.Handler(request));
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<ActionResult<CategoryModelOutPut>> Delete(Guid Id)
        {
            return CustomResponse(await _deleteCategory.Handler(Id));        
        }


    }
}
