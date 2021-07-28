namespace LegoM.Models.Merchants
{

using LegoM.Data;
using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Merchant;

    public class BecomeMerchantFormModel
    {
        [Required]
        [StringLength(NameMaxLength,MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Display(Name="Phone Number")]
        [Required]
        [StringLength(TelephoneNumberMaxLength, MinimumLength = TelephoneNumberMinLength)]
        public string TelephoneNumber { get; set; }
    }
}
