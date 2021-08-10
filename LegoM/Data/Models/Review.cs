namespace LegoM.Data.Models
{
    using LegoM.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Review;

    public class Review
    {
        public int Id { get; init; }

        [Required]
        public ReviewType Rating { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
         = new HashSet<Comment>();
    }
}
