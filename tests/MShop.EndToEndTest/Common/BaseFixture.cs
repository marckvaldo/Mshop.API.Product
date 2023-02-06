using Bogus;
using Microsoft.EntityFrameworkCore;
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

        protected APIClient _apiClient;

        protected CustomWebApplicationFactory<Program> _WebApp;

        protected HttpClient HttpClient;

        protected BaseFixture()
        {
            faker = new Faker("pt_BR");

            _WebApp = new CustomWebApplicationFactory<Program>();
            HttpClient = _WebApp.CreateClient();

            _apiClient = new APIClient(HttpClient);
        }

        protected RepositoryDbContext CreateDBContext(bool preserveData = false)
        {

            var context = new RepositoryDbContext(
                new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(Configuration.NameDataBase)
                .Options
                );

            if (!preserveData)
                context.Database.EnsureDeleted();

            return context;

        }

        protected void CleanInMemoryDatabase()
        {
            //CreateDBContext().Database.EnsureDeleted();
        }
    }
}
