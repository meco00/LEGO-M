namespace LegoM.Services.Orders.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static Data.DataConstants.Order;

    public class OrderFormServiceModel
    {

        [Display(Name = "Full Name")]
        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }

        [Required]
        [StringLength(AddressMаxLength, MinimumLength = AddressMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Address { get; init; }

        [Required]
        [StringLength(StateMаxLength, MinimumLength = StateMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string State { get; init; }

        [Required]
        [StringLength(CityMаxLength, MinimumLength = CityMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string City { get; init; }

        [Display(Name = "Zip Code")]
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "{0} must be in format 0000(4 digits)")]
        public string ZipCode { get; init; }


        [Display(Name = "Phone Number")]
        [Required]
        [RegularExpression(@"^08[789]\d{7}$", ErrorMessage = "{0} must be in format 08........")]
        public string TelephoneNumber { get; set; }

        

    }
}
