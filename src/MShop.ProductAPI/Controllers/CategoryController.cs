using Microsoft.AspNetCore.Mvc;
using MShop.Application.Common;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Application.UseCases.Category.DeleteCategory;
using MShop.Application.UseCases.Category.GetCatetory;
using MShop.Application.UseCases.Category.ListCategorys;
using MShop.Application.UseCases.Category.UpdateCategory;
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
        public CategoryController(
            IGetCategory getCategory,
            IListCategory listCategory,
            ICreateCategory createCategory,
            IUpdateCategory updateCategory,
            IDeleteCategory deleteCategory,
            INotification notification) : base(notification)
        {
            _getCategory = getCategory;
            _listCategory = listCategory;
            _createCategory = createCategory;
            _updateCategory = updateCategory;
            _deleteCategory = deleteCategory;   
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CategoryModelOutPut>> Category(Guid id)
        {
            try
            {
                return CustomResponse(await _getCategory.Handler(id));
            }
            catch (EntityValidationException)
            {
                return CustomResponse();
            }
            catch (Exception erro)
            {
                Notify(erro.Message);
                return CustomResponse();
            }

        }

        [HttpGet("list-category")]
        public async Task<ActionResult<List<CategoryModelOutPut>>> ListProdutcs()
        {
            try
            {
                return CustomResponse(await _listCategory.Handler());
            }
            catch(EntityValidationException)
            {
                return CustomResponse();
            }
            catch(Exception erro)
            {
                Notify(erro.Message);
                return CustomResponse();
            }
        }


        [HttpPost]
        public async Task<ActionResult<CategoryModelOutPut>> Create(CreateCategoryInPut request)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);
                return CustomResponse(await _createCategory.Handler(request));
            }
            catch(EntityValidationException)
            {
                return CustomResponse();
            }
            catch(Exception erro)
            {
                Notify(erro.Message);
                return CustomResponse();
            }
        }

        [HttpPut("{Id:Guid}")]
        public async Task<ActionResult<CategoryModelOutPut>> Update(Guid Id, UpdateCategoryInPut request)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                if (Id != request.Id)
                {
                    Notify("O id informado não é o mesmo passado como parametro");
                    return CustomResponse(request);
                }
                return CustomResponse(await _updateCategory.Handler(request));
            }
            catch(EntityValidationException)
            {
                return CustomResponse();
            }
            catch(Exception erro)
            {
                Notify(erro.Message);
                return CustomResponse();
            }
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<ActionResult<CategoryModelOutPut>> Delete(Guid Id)
        {
            try
            {
                return CustomResponse(await _deleteCategory.Handler(Id));
            }
            catch(EntityValidationException erro)
            {
                return CustomResponse();
            }
            catch(Exception erro)
            {
                Notify(erro.Message);
                return CustomResponse();
            }
        }
       
    }
}
