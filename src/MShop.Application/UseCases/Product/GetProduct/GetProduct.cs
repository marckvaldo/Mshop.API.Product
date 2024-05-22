using MShop.Application.UseCases.Category.Common;
using MShop.Core.Base;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public class GetProduct : BaseUseCase, IGetProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;

        public GetProduct(
            IProductRepository productRepository, 
            IImageRepository imageRepository, 
            INotification notification) : base(notification)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
        }

        public async Task<GetProductOutPut> Handle(GetProductInPut request, CancellationToken cancellation)
        {
            var product = await _productRepository.GetProductWithCategory(request.Id);            
            NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");

            var images = await _imageRepository.Filter(x => x.ProductId == product.Id);

            return new GetProductOutPut(
                product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Thumb?.Path,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                (new CategoryModelOutPut(product.CategoryId, product.Category.Name, product.Category.IsActive)),
                images.Select(x => x?.FileName).ToList(),
                product.IsSale) ;
        }
    }
}
