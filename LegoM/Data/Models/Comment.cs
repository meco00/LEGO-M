namespace LegoM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static DataConstants.Comment;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public DateTime PublishedOn { get; set; }

        public int ReviewId { get; set; }

        public Review Review { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }




    }
}
