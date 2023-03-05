using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MShop.Business.Entity;

namespace MShop.Repository.Mapping
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(16,2)");

            builder.Property(x => x.Stock)
                .IsRequired()
                .HasColumnType("decimal(16,2)");

            builder.OwnsOne(x=>x.Imagem, 
                x=>x.Property(x=>x.Path).HasColumnName("Imagem"));

            builder.ToTable("Products");
        }
    }


}
