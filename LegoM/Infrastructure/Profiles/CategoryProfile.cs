namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Products.Models;

    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<Category, ProductCategoryServiceModel>();
            this.CreateMap<SubCategory, ProductSubCategoryServiceModel>();
        }
    }
}
