namespace LegoM.Data
{
    using LegoM.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class LegoMDbContext : IdentityDbContext<User>
    {
        public LegoMDbContext(DbContextOptions<LegoMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<ProductImage> ProductsImages { get; set; }

        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
              .Entity<SubCategory>()
              .HasOne(x => x.Category)
              .WithMany(x => x.SubCategories)
              .HasForeignKey(x => x.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(x => x.SubCategory)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
             .HasOne(x => x.Category)
             .WithMany(x => x.Products)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
              .HasOne(x => x.Merchant)
              .WithMany(x => x.Products)
              .HasForeignKey(x => x.MerchantId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Merchant>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Merchant>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductImage>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

    }
}
