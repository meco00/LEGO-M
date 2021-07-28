namespace LegoM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Category;

    public class Category
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }

    }
}
