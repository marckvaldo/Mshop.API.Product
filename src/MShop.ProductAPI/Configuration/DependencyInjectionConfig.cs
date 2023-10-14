using Mshop.Cache.RepositoryRedis;
using MShop.Application.Event;
using MShop.Business.Interface;
using MShop.Business.Interface.Cache;
using MShop.Business.Interface.Event;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;

namespace MShop.ProductAPI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services) 
        {
            services.AddScoped<RepositoryDbContext>();
            services.AddScoped<IDomainEventPublisher,DomainEventPublisher>();            
            services.AddScoped<IUnitOfWork,UnitOfWork>();


            /*services.AddScoped<IGetProduct, GetProduct>();
            services.AddScoped<ICreateProduct, CreateProduct>();
            services.AddScoped<IUpdateProduct, UpdateProduct>();
            services.AddScoped<IDeleteProduct, DeleteProduct>();
            services.AddScoped<IUpdateStockProduct, UpdateStockProducts>();
            services.AddScoped<IListProducts, ListProducts>();
            services.AddScoped<IProductsPromotions, ProductsPromotions>();
            services.AddScoped<IUpdateThumb, UpdateThumb>();
            services.AddScoped<IGetCategory, GetCategory>();
            services.AddScoped<ICreateCategory, CreateCategory>();
            services.AddScoped<IUpdateCategory, UpdateCategory>();
            services.AddScoped<IDeleteCategory, DeleteCategory>();
            services.AddScoped<IListCategory, ListCategory>();
            services.AddScoped<IGetCategoryWithProducts, GetCategoryWithProducts>();
            services.AddScoped<IListImage, ListImage>();
            services.AddScoped<IGetImage, GetImage>();
            services.AddScoped<IDeleteImage, DeleteImage>();
            services.AddScoped<ICreateImage, CreateImage>();*/


            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImagesRepository>();

           
            services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssemblies(typeof(MShop.Application.UseCases.Category.CreateCategory.CreateCategory).Assembly);
                x.RegisterServicesFromAssemblies(typeof(MShop.Application.UseCases.Images.CreateImage.CreateImage).Assembly);
                x.RegisterServicesFromAssemblies(typeof(MShop.Application.UseCases.Product.CreateProducts.CreateProduct).Assembly);
            });

            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<INotification, Notifications>();
            services.AddScoped<ICacheRepository, RedisRepository>();


            return services;
        }
    }
}
