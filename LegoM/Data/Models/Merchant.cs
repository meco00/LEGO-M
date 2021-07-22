namespace LegoM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Merchant
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(MerchantNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MerchantTelephoneNumberMaxLength)]
        public string TelephoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }

    }
}
