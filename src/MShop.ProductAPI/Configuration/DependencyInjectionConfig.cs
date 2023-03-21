using Mshop.Cache.RepositoryRedis;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Application.UseCases.Category.DeleteCategory;
using MShop.Application.UseCases.Category.GetCatetory;
using MShop.Application.UseCases.Category.ListCategorys;
using MShop.Application.UseCases.Category.UpdateCategory;
using MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory;
using MShop.Application.UseCases.images.CreateImage;
using MShop.Application.UseCases.images.DeleteImage;
using MShop.Application.UseCases.images.GetImage;
using MShop.Application.UseCases.images.ListImage;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.UseCases.Product.DeleteProduct;
using MShop.Application.UseCases.Product.GetProduct;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Application.UseCases.Product.Productspromotions;
using MShop.Application.UseCases.Product.ProductsPromotions;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Business.Interface;
using MShop.Business.Interface.Cache;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;

namespace MShop.ProductAPI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services) 
        {
            services.AddScoped<RepositoryDbContext>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IGetProduct, GetProduct>();
            services.AddScoped<ICreateProduct, CreateProduct>();
            services.AddScoped<IUpdateProduct, UpdateProduct>();
            services.AddScoped<IDeleteProduct, DeleteProduct>();
            services.AddScoped<IUpdateStockProduct, UpdateStockProducts>();
            services.AddScoped<IListProducts, ListProducts>();
            services.AddScoped<IProductsPromotions, ProductsPromotions>();


            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGetCategory, GetCategory>();
            services.AddScoped<ICreateCategory, CreateCategory>();
            services.AddScoped<IUpdateCategory, UpdateCategory>();
            services.AddScoped<IDeleteCategory, DeleteCategory>();
            services.AddScoped<IListCategory, ListCategory>();
            services.AddScoped<IGetCategoryWithProducts, GetCategoryWithProducts>();

            services.AddScoped<IImageRepository, ImagesRepository>();
            services.AddScoped<IListImage, ListImage>();
            services.AddScoped<IGetImage, GetImage>();
            services.AddScoped<IDeleteImage, DeleteImage>();
            services.AddScoped<ICreateImage, CreateImage>();


            services.AddScoped<IStorageService, StorageService>();
            
            services.AddScoped<INotification, Notifications>();
            services.AddScoped<ICacheRepository, RedisRepository>();

            return services;
        }
    }
}
