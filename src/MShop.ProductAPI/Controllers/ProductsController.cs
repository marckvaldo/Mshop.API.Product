using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MShop.Business.Interface;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.UseCases.Product.GetProduct;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Application.UseCases.Product.DeleteProduct;
using MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.ProductsPromotions;
using MShop.Application.UseCases.Product.UpdateThumb;

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
        private readonly IUpdateThumb _updateThumb;

        public ProductsController(
            IGetProduct getProduct, 
            ICreateProduct createProduct, 
            IUpdateProduct updateProduct, 
            IDeleteProduct deleteProduct,
            IUpdateStockProduct updateStoqueProduct,
            IListProducts listProducts,
            INotification notification,
            IProductsPromotions productPromotions,
            IUpdateThumb updateThumb
            ) : base(notification)
        {
            _getProduct = getProduct;
            _createProduct = createProduct;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
            _updateStoqueProduct = updateStoqueProduct;
            _listProducts = listProducts;
            _productPromotions = productPromotions;
            _updateThumb = updateThumb;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<ProductModelOutPut>>> Product(Guid id)
        {
            return CustomResponse(await _getProduct.Handler(id));        
        }

        [HttpGet("list-products")]
        public async Task<ActionResult<List<ProductModelOutPut>>> ListProdutcs([FromQuery] ListProductInPut request)
        {
            return CustomResponse(await _listProducts.Handler(request));
        }

        [HttpGet("list-products-promotions")]
        public async Task<ActionResult<List<ProductModelOutPut>>> ListProdutcsPromotions()
        { 
            return CustomResponse(await _productPromotions.Handler());
        }

        [HttpPost]
        public async Task<ActionResult<ProductModelOutPut>> Create([FromBody] CreateProductInPut product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _createProduct.Handler(product, cancellationToken));      
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Update(Guid id, UpdateProductInPut product, CancellationToken cancellationToken)
        {
        
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (id != product.Id)
            {
                Notify("O id informado não é o mesmo passado como parametro");
                return CustomResponse(product);
            }

            return CustomResponse(await _updateProduct.Handler(product, cancellationToken));
        
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Delete(Guid id, CancellationToken cancellationToken)
        {
            return CustomResponse(await _deleteProduct.Handler(id, cancellationToken));
        }

        [HttpPost("update-stock/{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> UpdateStock(Guid id, [FromBody] UpdateStockProductInPut product, CancellationToken cancellationToken)
        {
       
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (id != product.Id)
            {
                Notify("O id informado não é o mesmo passado como parametro");
                return CustomResponse(product);
            }

            return CustomResponse(await _updateStoqueProduct.Handler(product, cancellationToken));
        
        }

        [HttpPut("update-thump/{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> UpdateThumb(Guid id, [FromBody] UpdateThumbInput product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) CustomResponse(ModelState);

            if(id != product.Id)
            {
                Notify("O id informado não é o mesmo passado como parametro");
                return CustomResponse(product);
            }

            return CustomResponse(await _updateThumb.Handler(product, cancellationToken));
        }
    }
}

