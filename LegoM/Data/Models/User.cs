namespace LegoM.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.User;

    public class User:IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        public virtual ICollection<Review> Reviews { get; init; }
        = new HashSet<Review>();

        public virtual ICollection<Question> Questions { get; set; }
         = new HashSet<Question>();

        public virtual ICollection<Comment> Comments { get; set; }
        = new HashSet<Comment>();

        public virtual ICollection<Answer> Answers { get; set; }
        = new HashSet<Answer>();

        public virtual ICollection<Favourite> Favourites { get; set; }
       = new HashSet<Favourite>();
    }
}
