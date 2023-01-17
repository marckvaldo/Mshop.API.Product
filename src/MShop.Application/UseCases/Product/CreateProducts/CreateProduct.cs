using MShop.Application.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
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
            var product = new Business.Entity.Product(request.Description,
                    request.Name,
                    request.Price,
                    request.Imagem,
                    request.CategoryId,
                    request.Stock,
                    request.IsActive
                );

            product.IsValid(_notifications);

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
