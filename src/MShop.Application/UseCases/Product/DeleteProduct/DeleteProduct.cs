using MShop.Application.UseCases.Product.Common;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;


namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProduct : BaseUseCase, IDeleteProduct
    {
        private readonly IProductRepository _productRespository;
        private readonly IImageRepository _imageRepository;
 
        public DeleteProduct(IProductRepository productRespository, 
            IImageRepository imageRepository,
            INotification notification):base(notification)
        {
            _productRespository = productRespository;
            _imageRepository = imageRepository;
        }

        public async Task<ProductModelOutPut> Handler(Guid request)
        {
            var product = await _productRespository.GetById(request);

            if (product is null)
            {
                Notify($"Produto com id {request} não encontrado");
                throw new ApplicationValidationException("");
            }

            //implementar o delete images

            await _productRespository.DeleteById(product);
            await _imageRepository.DeleteByIdProduct(product.Id);

            return new ProductModelOutPut(
                product.Id, 
                product.Description,
                product.Name, 
                product.Price, 
                product.Thumb?.Path, 
                product.Stock, 
                product.IsActive, 
                product.CategoryId);
        }
    }
}
