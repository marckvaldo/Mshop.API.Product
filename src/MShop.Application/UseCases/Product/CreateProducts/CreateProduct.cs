using MShop.Application.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Validation;
using MShop.Business.Validator;
using System.Security.Cryptography.X509Certificates;
using Business = MShop.Business.Entity;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public class CreateProduct : BaseUseCase, ICreateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly INotification _notifications;

        public CreateProduct(IProductRepository productRepository, INotification notification) : base(notification)
        {
            _productRepository = productRepository;
            _notifications = notification;
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

            if(!IsValid(new ProductValidador(product, _notifications)))
            {
                return null;
            }

            await _productRepository.Create(product);
            var newProduct = await _productRepository.GetLastRegister(x=>x.Name == request.Name);
            

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
