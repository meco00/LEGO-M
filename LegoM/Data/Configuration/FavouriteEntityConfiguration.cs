namespace LegoM.Data.Configuration
{
    using LegoM.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FavouriteEntityConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {

          builder
                .HasOne(x => x.Product)
                .WithMany(x => x.Favourites)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

         builder
             .HasOne(x => x.User)
             .WithMany(x => x.Favourites)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
