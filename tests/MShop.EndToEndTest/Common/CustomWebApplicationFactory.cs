using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using MShop.Repository.Context;


namespace MShop.EndToEndTest.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            //aqui vai ler os dados de appsettings.EndToEndTest.json

            builder.UseEnvironment("EndToEndTest");
            builder.ConfigureServices(Services =>
            {

                if(Configuration.DATABASE_MEMORY)
                {

                    var dbOption = Services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<RepositoryDbContext>));

                    if (dbOption is not null)
                        Services.Remove(dbOption);

                    Services.AddDbContext<RepositoryDbContext>(Options =>
                    {
                        Options.UseInMemoryDatabase(Configuration.NAME_DATA_BASE);
                    });


                    var cacheOption = Services.FirstOrDefault(x => x.ServiceType == typeof(IDistributedCache));

                    if (cacheOption is not null)
                        Services.Remove(cacheOption);

                    Services.AddDistributedMemoryCache();

                }
                else
                {
                    
                    var servicesProvides = Services.BuildServiceProvider();
                    
                    using (var scope = servicesProvides.CreateScope())
                    {
                        //colocar aqui o rabittMQ
                        var context = scope.ServiceProvider.GetService<RepositoryDbContext>();
                        ArgumentNullException.ThrowIfNull(context);
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                }
            });

            base.ConfigureWebHost(builder);
        }
    }
}
