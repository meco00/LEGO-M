namespace LegoM.Data.Configuration
{
    using LegoM.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

          builder
                .HasOne(x => x.Review)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);


         builder
              .HasOne(x => x.User)
              .WithMany(x => x.Comments)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
