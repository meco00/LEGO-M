namespace LegoM.Data.Models
{
    using LegoM.Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Report;

    public class Report
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
