using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected BaseFixture()
        {
            faker = new Faker("pt_BR");

            webApp = new CustomWebApplicationFactory<Program>();
            httpClient = webApp.CreateClient();

            apiClient = new APIClient(httpClient);

            //pegando o services alterado do CustomWebApplicationFactory e recuperando a connectString
            var configurationServer = webApp.Services.GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configurationServer);
            _connectionString = ((IConfiguration)configurationServer).GetConnectionString("RepositoryMysql");
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

                if (!preserveData)
                    context.Database.EnsureDeleted();

                return context;
            }

           

        }

        protected void CleanInMemoryDatabase()
        {
            //CreateDBContext().Database.EnsureDeleted();
        }
    }
}
