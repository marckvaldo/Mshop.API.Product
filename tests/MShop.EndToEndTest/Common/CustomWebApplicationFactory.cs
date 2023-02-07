using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(Services =>
            {
                var dbOption  = Services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<RepositoryDbContext>));

                if(dbOption is not null)
                    Services.Remove(dbOption);

                Services.AddDbContext<RepositoryDbContext>(Options =>
                {
                    Options.UseInMemoryDatabase(Configuration.NameDataBase);
                });

            });
        }
    }
}
