namespace LegoM.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static DataConstants.Trader;

    public class Trader
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(TelephoneNumberMaxLength)]
        public string TelephoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }

    }
}
