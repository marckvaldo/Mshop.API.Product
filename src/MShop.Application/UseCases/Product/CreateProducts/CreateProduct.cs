using MShop.Application.UseCases.Product.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.ValueObject;
using Business = MShop.Business.Entity;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public class CreateProduct : BaseUseCase, ICreateProduct
    {
        private readonly IProductRepository _productRepository;
        public CreateProduct(IProductRepository productRepository, INotification notification) : base(notification)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductModelOutPut> Handle(CreateProductInPut request)
        {
            var product = new Business.Entity.Product(
                    request.Description,
                    request.Name,
                    request.Price,
                    request.CategoryId,
                    request.Stock,
                    request.IsActive
                );

            product.UpdateImage(request.Imagem);

            product.IsValid(_notifications);

            await _productRepository.Create(product);          

            return new ProductModelOutPut(
                    product.Id,
                    product.Description,
                    product.Name,
                    product.Price,
                    product.Imagem?.Path,
                    product.Stock,
                    product.IsActive,
                    product.CategoryId
                );
        }
    }
}
