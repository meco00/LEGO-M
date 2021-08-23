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
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Trader> Merchants { get; set; }

        public DbSet<ProductImage> ProductsImages { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Favourite> Favourites { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCarts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Report> Reports { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.ApplyConfiguration(new ProductEntityConfiguration());

            builder.ApplyConfiguration(new ReviewEntityConfiguration());

            builder.ApplyConfiguration(new QuestionEntityConfiguration());

            builder.ApplyConfiguration(new AnswerEntityConfiguration());

            builder.ApplyConfiguration(new CommentEntityConfiguration());

            builder.ApplyConfiguration(new FavouriteEntityConfiguration());

            builder.ApplyConfiguration(new ShoppingCartItemEntityConfiguration());

            builder.ApplyConfiguration(new ReportEntityConfiguration());

            builder
              .Entity<SubCategory>()
              .HasOne(x => x.Category)
              .WithMany(x => x.SubCategories)
              .HasForeignKey(x => x.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Trader>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Trader>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductImage>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(builder);
        }

    }
}
