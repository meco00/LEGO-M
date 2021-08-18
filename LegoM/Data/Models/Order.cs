namespace LegoM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Order;

    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(AddressMаxLength)]
        public string Address { get; set; }

        [Required]
        [MaxLength(CityMаxLength)]
        public string City { get; set; }

        [Required]
        [MaxLength(StateMаxLength)]
        public string State { get; set; }

        [Required]
        [MaxLength(ZipCodeMaxLength)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(TelephoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        public DateTime OrderedOn { get; set; }

        public bool IsAccomplished { get; set; }

        public virtual ICollection<ShoppingCartItem> ShoppingCart { get; set; }
        = new HashSet<ShoppingCartItem>();

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }




    }
}
