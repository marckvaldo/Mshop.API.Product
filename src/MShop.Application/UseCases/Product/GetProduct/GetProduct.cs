using MShop.Application.Common;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public class GetProduct : BaseUseCase, IGetProduct
    {
        private readonly IProductRepository _productRepository;

        public GetProduct(IProductRepository productRepository, INotification notification) : base(notification)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductModelOutPut?> Handle(Guid Id)
        {
            var product = await _productRepository.GetById(Id);

            if (product == null)
            {
                Notify("Não possivel localizar produto na base de dados");
                throw new EntityValidationException("There are erros", Errors());
            }

            return new ProductModelOutPut(
                product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Imagem, 
                product.Stock, 
                product.IsActive, 
                product.CategoryId);
        }
    }
}
