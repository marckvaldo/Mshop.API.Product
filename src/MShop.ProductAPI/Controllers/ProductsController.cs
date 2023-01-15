using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.UseCases.Product.GetProduct;
using MShop.Application.Common;
using MShop.Application.UseCases.Product.UpdateProducts;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Application.UseCases.Product.DeleteProduct;

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

        public ProductsController(
            IGetProduct getProduct, 
            ICreateProduct createProduct, 
            IUpdateProduct updateProduct, 
            IDeleteProduct deleteProduct, 
            INotification notification) : base(notification)
        {
            _getProduct = getProduct;
            _createProduct = createProduct;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<ProductModelOutPut>>> Product(Guid id)
        {
            var produtcs = await _getProduct.Handle(id);
            if (produtcs == null) return NotFound();
            return CustomResponse(produtcs);
        }

        [HttpPost]
        public async Task<ActionResult<ProductModelOutPut>> Create([FromBody] CreateProductInPut product)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var newProduct = await _createProduct.Handle(product);
            //if(newProduct == null) return NotFound();
            return CustomResponse(newProduct);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Update(Guid Id, UpdateProductInPut product)
        {
            if (Id != product.Id) return BadRequest();
            var newProduct = await _updateProduct.Handle(product);
            if (newProduct == null) return NotFound();
            return CustomResponse(newProduct);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Delete(Guid Id)
        {
            var product = await _getProduct.Handle(Id);
            if (product == null) return NotFound();
            await _deleteProduct.Handle(Id);
            return CustomResponse(product);
        }



    }
}

