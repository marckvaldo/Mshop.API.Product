using MShop.Application.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Validation;
using System.Security.Cryptography.X509Certificates;
using Business = MShop.Business.Entity;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public class CreateProduct : BaseUseCase, ICreateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;

        public CreateProduct(IProductRepository productRepository, INotification _notification) : base(_notification)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductModelOutPut> Handle(CreateProductInPut request)
        {
            var product = new Business.Entity.Product(
                    request.Description,
                    request.Name,
                    request.Price,
                    request.Imagem,
                    request.CategoryId,
                    request.Stock,
                    request.IsActive
                );

            await _productRepository.Create(product);
            var newProduct = await _productRepository.GetLastRegister(x=>x.Name == request.Name);
            if (newProduct == null) return null;

            return new ProductModelOutPut(
                    newProduct.Id,
                    request.Description,
                    request.Name,
                    request.Price,
                    request.Imagem,
                    request.Stock,
                    request.IsActive,
                    request.CategoryId
                );
        }
    }
}
