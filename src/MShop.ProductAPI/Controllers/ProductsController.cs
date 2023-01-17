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
using MShop.Business.Exceptions;
using MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Application.UseCases.Product.ListProducts;

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
        private readonly IUpdateStockProduct _UpdateStoqueProduct;
        private readonly IListProducts _ListProducts;

        public ProductsController(
            IGetProduct getProduct, 
            ICreateProduct createProduct, 
            IUpdateProduct updateProduct, 
            IDeleteProduct deleteProduct,
            IUpdateStockProduct updateStoqueProduct,
            IListProducts listProducts,
            INotification notification
            ) : base(notification)
        {
            _getProduct = getProduct;
            _createProduct = createProduct;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
            _UpdateStoqueProduct = updateStoqueProduct;
            _ListProducts = listProducts;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<ProductModelOutPut>>> Product(Guid id)
        {
            try
            {
                return CustomResponse(await _getProduct.Handle(id));
            }
            catch(EntityValidationException error)
            {
                return CustomResponse(error.Errors);
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);
            }

        }

        [HttpGet("list-produtcs")]
        public async Task<ActionResult<List<ProductModelOutPut>>> ListProdutcs()
        {
            try
            {
                return CustomResponse(await _ListProducts.Handle());
            }
            catch (EntityValidationException error)
            {
                return CustomResponse(error.Errors);
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);
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
            catch(EntityValidationException error)
            {
                return CustomResponse(error.Errors) ;
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);
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
            catch(EntityValidationException error)
            {
                return CustomResponse(error.Errors);
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductModelOutPut>> Delete(Guid Id)
        {
            try
            {
                return CustomResponse(await _deleteProduct.Handle(Id));
            }
            catch(EntityValidationException error)
            {
                return CustomResponse(error.Errors);
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

                return CustomResponse(await _UpdateStoqueProduct.Handle(product));
            }
            catch (EntityValidationException error)
            {
                return CustomResponse(error.Errors);
            }
            catch (Exception error)
            {
                Notify(error.Message);
                return CustomResponse(error);
            }
        }
    }
}

