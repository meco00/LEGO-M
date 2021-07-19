namespace LegoM.Data.Models
{


    public class ProductSubCategory
    {
        public string ProductId { get; set; }

        public string SubCategoryId { get; set; }

        public virtual Product Product { get; set; }

        public virtual SubCategory SubCategory { get; set; }
    }
}
