namespace LegoM.Data
{
    using LegoM.Data.Configuration;
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

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.ApplyConfiguration(new ProductEntityConfiguration());

            builder.ApplyConfiguration(new ReviewEntityConfiguration());

            builder.ApplyConfiguration(new QuestionEntityConfiguration());

            builder.ApplyConfiguration(new AnswerEntityConfiguration());

            builder
              .Entity<SubCategory>()
              .HasOne(x => x.Category)
              .WithMany(x => x.SubCategories)
              .HasForeignKey(x => x.CategoryId)
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

          

        

         


           



            base.OnModelCreating(builder);
        }

    }
}
