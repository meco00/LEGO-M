namespace LegoM.Data.Models
{
    using LegoM.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Product;

    public class Product
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TittleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte Quantity { get; set; } 


        [Required]
        public ProductCondition ProductCondition { get; set; }

        [Required]
        public DeliveryTake DeliveryTake { get; set; }

        public DateTime PublishedOn { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string MerchantId { get; set; }

        public Merchant Merchant { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public string SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

       

        //public virtual ICollection<Question> Questions { get; set; }

        //public virtual ICollection<UserReview> UserReviews { get; set; }

        //public virtual ICollection<UserProduct> UserProducts { get; set; }

        //public virtual ICollection<Report> Reports { get; set; }

        //public virtual ICollection<Picture> Pictures { get; set; }
    }
}
