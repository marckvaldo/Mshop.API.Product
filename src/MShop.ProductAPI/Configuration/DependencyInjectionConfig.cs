using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.UseCases.Product.DeleteProduct;
using MShop.Application.UseCases.Product.GetProduct;
using MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;

namespace MShop.ProductAPI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDepencies(this IServiceCollection services) 
        {
            services.AddScoped<RepositoryDbContext>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IGetProduct, GetProduct>();
            services.AddScoped<ICreateProduct, CreateProduct>();
            services.AddScoped<IUpdateProduct, UpdateProduct>();
            services.AddScoped<IDeleteProduct, DeleteProduct>();


            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INotification, Notifications>();



            return services;
        }
    }
}
