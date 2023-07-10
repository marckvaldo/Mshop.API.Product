using MShop.ProductAPI.Filter;
using System.Text.Json.Serialization;

namespace MShop.ProductAPI.Configuration
{
    public static class ControllerConfiguration
    {
        public static IServiceCollection AddAndConfigureController(this IServiceCollection services)
        {
            services.AddControllers(
                //aqui eu forço toda exception passar por aqui 
                options => options.Filters.Add(typeof(ApiGlobalExceptionFilter))
                )
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDocumentation();
            return services;
        }

        public static IServiceCollection AddDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseDocumentation(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    
    }
}
