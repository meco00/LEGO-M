namespace LegoM.Models.Products
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Product;

    public class ProductFormModel
    {
        [Required]
        [StringLength(TittleMaxLength,MinimumLength = TittleMinLength,ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Title { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Description { get; init; }

        [Range(PriceMinLength, PriceMaxLength,ErrorMessage ="The field {0} must be between {1:f2} and {2}")]
        public decimal Price { get; init; }

        [Range(QuantityMinLength,byte.MaxValue)]
        public byte Quantity { get; init; }


        [Required]
        [Display(Name = "First Image Url")]
        [Url]
        public string FirstImageUrl { get; set; }

        [Display(Name = "Second Image Url(Optional)")]
        [Url]
        public string SecondImageUrl { get; set; }

        [Display(Name = "Third Image Url(Optional)")]
        [Url]
        public string ThirdImageUr { get; set; }

        [Required(ErrorMessage = "Please select an Category from the list.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please select an subCategory from the list.")]
        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Please select an condition from the list.")]
        [EnumDataType(typeof(ProductCondition))]
        public ProductCondition? Condition { get; set; }

        [Required(ErrorMessage = "Please select an delivery taker from the list.")]
        [EnumDataType(typeof(DeliveryTake))]
        public DeliveryTake? Delivery { get; set; }

        public bool AgreeOnTermsOfPolitics { get; set; }

        public IEnumerable<ProductCategoryServiceModel> Categories { get; set; }

        public IEnumerable<ProductSubCategoryServiceModel> SubCategories { get; set; }


    }
}
