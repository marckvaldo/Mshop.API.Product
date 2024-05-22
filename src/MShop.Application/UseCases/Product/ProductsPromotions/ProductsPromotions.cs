using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.ProductsPromotions;
using MShop.Core.Base;
using MShop.Core.Message;
using MShop.Repository.Interface;
using MShop.Core.Cache;


namespace MShop.Application.UseCases.Product.Productspromotions
{
    public class ProductsPromotions : BaseUseCase, IProductsPromotions
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly IProductRepository _productRepository;

        public ProductsPromotions(ICacheRepository cacheRepository, 
            IProductRepository productRepository, 
            INotification notification) : base(notification)
        {
            _cacheRepository = cacheRepository;
            _productRepository = productRepository;
        }

        public async Task<List<ProductModelOutPut>> Handler()
        {
            var productsCache = await _cacheRepository.GetKeyCollection<ProductModelOutPut>("promocao");
            if (productsCache is not null)
            {
                return productsCache!;
            }
                
            List<ProductModelOutPut> listProducts = new();

            var products = await _productRepository.GetProductsPromotions();
            foreach(var item in products)
            {
                listProducts.Add(new ProductModelOutPut(
                    item.Id,
                    item.Description,
                    item.Name,
                    item.Price,
                    item.Thumb?.Path,
                    item.Stock,
                    item.IsActive,
                    item.CategoryId));
            }

            await _cacheRepository.SetKeyCollection("promocao", listProducts, TimeSpan.FromMinutes(10));
                
            return listProducts;
        }

    }
}
