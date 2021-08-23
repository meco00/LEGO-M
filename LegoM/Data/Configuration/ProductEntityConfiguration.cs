namespace LegoM.Data.Configuration
{
    using LegoM.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductEntityConfiguration: IEntityTypeConfiguration<Product>
    {     

        public void Configure(EntityTypeBuilder<Product> builder)
        {
          
            builder
                 .HasOne(x => x.SubCategory)
                 .WithMany(x => x.Products)
                 .HasForeignKey(x => x.SubCategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

           builder               
                 .HasOne(x => x.Category)
                 .WithMany(x => x.Products)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder               
                  .HasOne(x => x.Trader)
                  .WithMany(x => x.Products)
                  .HasForeignKey(x => x.TraderId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
