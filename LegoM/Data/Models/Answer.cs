namespace LegoM.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Answer;

    public class Answer
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public DateTime PublishedOn { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User {get;set;}


    }
}
