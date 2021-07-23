namespace LegoM.Models.Merchants
{

using LegoM.Data;
using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class BecomeMerchantFormModel
    {
        [Required]
        [StringLength(MerchantNameMaxLength,MinimumLength =MerchantNameMinLength)]
        public string Name { get; set; }

        [Display(Name="Phone Number")]
        [Required]
        [StringLength(MerchantTelephoneNumberMaxLength, MinimumLength = MerchantTelephoneNumberMinLength)]
        public string TelephoneNumber { get; set; }
    }
}
