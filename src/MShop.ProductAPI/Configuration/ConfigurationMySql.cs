using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationMySql
    {
        public static IServiceCollection AddConfigurationMySql(this IServiceCollection services, IConfiguration configuration)
        {
            //configurando a conexao Mysql
            
            var ConnectionString = configuration.GetConnectionString("RepositoryMysql");
            services.AddDbContext<RepositoryDbContext>(options =>
                options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));

    
            return services;
        }

        //aqui analisa as migrations no projeto repository e executa as mesmas no banco de dados apenas isso.
        //aqui não cria migrations isso fica a cargo do desenvolvedor.
        public static WebApplication AddMigrateDatabase(this WebApplication app)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "EndToEndTest") return app;

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();
            dbContext.Database.Migrate();
            return app;
        }
    }
}
