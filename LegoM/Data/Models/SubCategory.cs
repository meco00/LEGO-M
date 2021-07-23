﻿namespace LegoM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class SubCategory
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<ProductSubCategory> ProductsSubCategories { get; set; }
    }
}