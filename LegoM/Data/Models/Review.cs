namespace LegoM.Data.Models
{
    using LegoM.Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Review;

    public class Review
    {
        public int Id { get; set; }

        [Required]
        public ReviewType Rating { get; set; }

        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public DateTime PublishedOn { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }


    }
}
