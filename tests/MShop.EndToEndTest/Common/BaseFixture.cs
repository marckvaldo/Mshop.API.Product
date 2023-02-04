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
        protected BaseFixture()
        {
            faker = new Faker("pt_BR"); 
        }

        protected RepositoryDbContext CreateDBContext(bool preserveData = false)
        {

            var context = new RepositoryDbContext(
                new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase("end2end-test-db")
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
