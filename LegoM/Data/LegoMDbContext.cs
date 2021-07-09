namespace LegoM.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LegoM.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class LegoMDbContext : IdentityDbContext
    {
        public LegoMDbContext(DbContextOptions<LegoMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Product>()
                .HasOne(x => x.SubCategory)
                .WithMany(x => x.Products)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<SubCategory>()
                .HasOne(x => x.Category)
                .WithMany(x => x.SubCategories)
                .OnDelete(DeleteBehavior.Restrict);



            base.OnModelCreating(builder);
        }

    }
}
