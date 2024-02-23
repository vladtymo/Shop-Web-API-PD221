using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductEntityConfigs : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Products");

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);

            builder.Property(x => x.CategoryId).HasDefaultValue(9); // "Other
                                                                 // 
            builder.HasMany(x => x.Orders).WithMany(x => x.Products);
        }
    }
}
