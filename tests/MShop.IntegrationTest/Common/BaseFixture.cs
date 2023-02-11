﻿using Bogus;
using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Common
{
    public abstract class BaseFixture
    {
        protected readonly Faker faker;
        protected static readonly Faker fakerStatic = new Faker("pt_BR");
        protected BaseFixture()
        {
            faker = new Faker("pt_BR"); 
        }

        protected RepositoryDbContext CreateDBContext(bool preserveData = false, string? dataBase = null)
        {
            if(dataBase is null)
               dataBase = Configuration.NameDataBase;

            var context = new RepositoryDbContext(
                new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(dataBase)
                .Options
                );

            if (!preserveData)
                context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            return context;

        }

        protected void CleanInMemoryDatabase(RepositoryDbContext context = null)
        {

            if(context is null)
            {
                CreateDBContext().Database.EnsureDeleted();
            }
            else
            {
                context.Database.EnsureDeleted();
            }
            
            
        }
    }
}
