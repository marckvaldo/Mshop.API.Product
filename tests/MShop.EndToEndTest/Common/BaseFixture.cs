using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace MShop.EndToEndTest.Common
{
    public abstract class BaseFixture
    {
        protected readonly Faker faker;
        protected static readonly Faker fakerStatic = new Faker("pt_BR");
        protected APIClient apiClient;
        protected CustomWebApplicationFactory<Program> webApp;
        protected HttpClient httpClient;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        protected BaseFixture()
        {
            faker = new Faker("pt_BR");

            webApp = new CustomWebApplicationFactory<Program>();
            httpClient = webApp.CreateClient();

            apiClient = new APIClient(httpClient);

            //pegando o services alterado do CustomWebApplicationFactory e recuperando a connectString e grando um 
            var configurationServer = webApp.Services.GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configurationServer);
            _configuration = ((IConfiguration)configurationServer);
            _connectionString = _configuration.GetConnectionString("RepositoryMysql");
        }

        protected RepositoryDbContext CreateDBContext(bool preserveData = false)
        {
            if(Configuration.DATABASE_MEMORY)
            {
                var context = new RepositoryDbContext(
                new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(Configuration.NAME_DATA_BASE)
                .Options
                );

                if (!preserveData)
                    context.Database.EnsureDeleted();

                return context;
            }
            else
            {
                var context = new RepositoryDbContext(
                   new DbContextOptionsBuilder<RepositoryDbContext>()
                   .UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString))
                   .Options);

                return context;
            }
        }

        protected IDistributedCache CreateCache()
        {
            if (Configuration.DATABASE_MEMORY)
            {
                var services = new ServiceCollection();
                services.AddDistributedMemoryCache();
                var provider = services.BuildServiceProvider();
                var memoryCache = provider.GetService<IDistributedCache>();
                ArgumentNullException.ThrowIfNull(memoryCache) ;
                return memoryCache;
            }
            else
            {
                var redisPassword = _configuration["Redis:Password"];
                var redisEndPoint = _configuration["Redis:Endpoint"];

                var services = new ServiceCollection();
                services.AddStackExchangeRedisCache(options =>
                {
                    options.InstanceName = "Redis";
                    options.Configuration = redisEndPoint;
                    if (!string.IsNullOrEmpty(redisPassword))
                    {
                        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions { Password = redisPassword };
                        options.ConfigurationOptions.EndPoints.Add(redisPassword);
                    }
                });

                var provider = services.BuildServiceProvider();
                var memoryCache = provider.GetService<IDistributedCache>();
                ArgumentNullException.ThrowIfNull(memoryCache) ;
                return memoryCache;
            }
        }

        protected void CleanInMemoryDatabase()
        {
            //CreateDBContext().Database.EnsureDeleted();
        }
    }
}
