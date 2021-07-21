namespace LegoM.Models.Products
{
    using LegoM.Data.Models.Enums;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddProductFormModel
    {
        [Required]
        [StringLength(ProductTittleMaxLength,MinimumLength = ProductTittleMinLength,ErrorMessage = "The field {0} must be between {1} and {2} characters long")]
        public string Title { get; init; }

        [Required]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Description { get; init; }

        [Range(ProductPriceMinLength, ProductPriceMaxLength,ErrorMessage ="The field {0} must be between {1:f2} and {2}")]
        public decimal Price { get; init; }

        [Range(ProductQuantityMinLength,byte.MaxValue)]
        public byte Quantity { get; init; }


        [Required]
        [Display(Name = "SubCategories")]
        public IEnumerable<string> SubCategoriesIds { get; init; } = new List<string>();

        [Required(ErrorMessage = "Select an item from the list.")]
        [EnumDataType(typeof(ProductCondition))]
        public ProductCondition? Condition { get; set; }

        [Required(ErrorMessage = "Select an item from the list.")]
        [EnumDataType(typeof(DeliveryTake))]
        public DeliveryTake? Delivery { get; set; }

        public bool AgreeOnTermsOfPolitics { get; set; }

        public IEnumerable<ProductSubCategoryViewModel> SubCategories { get; set; }


    }
}
