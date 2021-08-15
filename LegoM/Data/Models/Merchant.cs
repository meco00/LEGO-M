namespace LegoM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Merchant;

    public class Merchant
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

        public virtual User User {get;init;}

        public virtual IEnumerable<Product> Products { get; set; }

    }
}
