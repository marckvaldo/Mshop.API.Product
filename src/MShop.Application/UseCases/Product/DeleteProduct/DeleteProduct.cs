using MShop.Application.UseCases.Product.Common;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProduct : BaseUseCase, IDeleteProduct
    {
        private readonly IProductRepository _productRespository;
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
 
        public DeleteProduct(IProductRepository productRespository, 
            IImageRepository imageRepository,
            INotification notification,
            IStorageService storageService):base(notification)
        {
            _productRespository = productRespository;
            _imageRepository = imageRepository;
            _storageService = storageService;
        }

        public async Task<ProductModelOutPut> Handler(Guid request)
        {
            var product = await _productRespository.GetById(request);

            /*if (product is null)
            {
                Notify($"Produto com id {request} não encontrado");
                throw new ApplicationValidationException("");
            }*/

            Notify("Não foi possivel localizar a produto da base de dados!");
            NotFoundException.ThrowIfnull(product, "your search returned null");


            await _productRespository.DeleteById(product);
            await _imageRepository.DeleteByIdProduct(product.Id);
            if(product?.Thumb?.Path is not null)
                await _storageService.Delete(product.Thumb.Path);

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
