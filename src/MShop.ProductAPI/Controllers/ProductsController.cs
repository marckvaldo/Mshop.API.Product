using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MShop.Business.Interface;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.UseCases.Product.GetProduct;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Application.UseCases.Product.DeleteProduct;
using MShop.Business.Exceptions;
using MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.ProductsPromotions;

namespace MShop.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : MainController
    {
        private readonly IGetProduct _getProduct;
        private readonly ICreateProduct _createProduct;
        private readonly IUpdateProduct _updateProduct;
        private readonly IDeleteProduct _deleteProduct;
        private readonly IUpdateStockProduct _updateStoqueProduct;
        private readonly IListProducts _listProducts;
        private readonly IProductsPromotions _productPromotions;

        public ProductsController(
            IGetProduct getProduct, 
            ICreateProduct createProduct, 
            IUpdateProduct updateProduct, 
            IDeleteProduct deleteProduct,
            IUpdateStockProduct updateStoqueProduct,
            IListProducts listProducts,
            INotification notification,
            IProductsPromotions productPromotions
            ) : base(notification)
        {
            _getProduct = getProduct;
            _createProduct = createProduct;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
            _updateStoqueProduct = updateStoqueProduct;
            _listProducts = listProducts;
            _productPromotions = productPromotions;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<ProductModelOutPut>>> Product(Guid id)
        {
            try
            {
                return CustomResponse(await _getProduct.Handle(id));
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse();
            }

        }

        [HttpGet("list-products")]
        public async Task<ActionResult<List<ProductModelOutPut>>> ListProdutcs([FromQuery] ListProductInPut request)
        {
            try
            {
                return CustomResponse(await _listProducts.Handle(request));
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse();
            }
        }

        [HttpGet("list-products-promotions")]
        public async Task<ActionResult<List<ProductModelOutPut>>> ListProdutcsPromotions()
        {
            try
            {
                return CustomResponse(await _productPromotions.Handle());
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductModelOutPut>> Create([FromBody] CreateProductInPut product)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);
                return CustomResponse(await _createProduct.Handle(product));
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse();
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Update(Guid Id, UpdateProductInPut product)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                if (Id != product.Id)
                {
                    Notify("O id informado não é o mesmo passado como parametro");
                    return CustomResponse(product);
                }

                return CustomResponse(await _updateProduct.Handle(product));
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Delete(Guid Id)
        {
            try
            {
                return CustomResponse(await _deleteProduct.Handle(Id));
            }
            catch(Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);   
            }
        }


        [HttpPost("update-stock/{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> UpdateStock(Guid Id, [FromBody] UpdateStockProductInPut product)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                if (Id != product.Id)
                {
                    Notify("O id informado não é o mesmo passado como parametro");
                    return CustomResponse(product);
                }

                return CustomResponse(await _updateStoqueProduct.Handle(product));
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);
            }
        }
    }
}

