using Microsoft.EntityFrameworkCore;
using MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Repository.Context
{
    public class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categorys { get; set; }


        //quando inicar a criação ele vai pegar todas as classes que herdam IEntityTypeConfiguration

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach(var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e=>e.GetProperties()
                .Where(p=>p.ClrType == typeof(string))))
            {
                property.SetColumnType("Varchar(100)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);

            //aque eu estou retirando o modo cascata
            foreach(var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }

        
    }
}
