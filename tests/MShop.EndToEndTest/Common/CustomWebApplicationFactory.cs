using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MShop.Messaging.Configuration;
using MShop.Repository.Context;
using RabbitMQ.Client;

namespace MShop.EndToEndTest.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IModel RabbitMQChannel { get; private set; }
        public IOptions<RabbitMQConfiguration> RabbitMQConfiguration { get; private set; }
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
                        RabbitMQChannel = scope
                        .ServiceProvider
                        .GetService<ChannelManager>()!
                        .GetChannel();

                        RabbitMQConfiguration = scope
                        .ServiceProvider
                        .GetService<IOptions<RabbitMQConfiguration>>()!;   


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
