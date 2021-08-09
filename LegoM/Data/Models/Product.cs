﻿namespace LegoM.Data.Models
{
    using LegoM.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
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

        public virtual Merchant Merchant { get; set; }

        
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        
        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        [NotMapped]
        public  string MainImageUrl
        {
            get
            {
                return Images.Select(x => x.ImageUrl).FirstOrDefault();
            }

           
        }

        [NotMapped]
        public string SecondImageUrl
        {
            get
            {
                return Images.Skip(1).Take(1).Select(x => x.ImageUrl).FirstOrDefault();
            }
        }

        [NotMapped]
        public string ThirdImageUrl
        {
            get
            {
                return Images.Skip(2).Take(1).Select(x => x.ImageUrl).FirstOrDefault();
            }
        }

        public virtual ICollection<ProductImage> Images { get; init; } 
          = new HashSet<ProductImage>();

        public virtual ICollection<Review> Reviews { get; init; }
          = new HashSet<Review>();

        public virtual ICollection<Question> Questions { get; set; }
          = new HashSet<Question>();


        //public virtual ICollection<UserProduct> UserProducts { get; set; }

        //public virtual ICollection<Report> Reports { get; set; }


    }
}
