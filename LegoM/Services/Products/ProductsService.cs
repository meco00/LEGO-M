namespace LegoM.Services.Products
{
  using LegoM.Data;
    using LegoM.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductsService : IProductsService
    {
        private readonly LegoMDbContext data;

        public ProductsService(LegoMDbContext data)
        => this.data = data;

        public ProductQueryServiceModel All(string category,
            string searchTerm,
            int currentPage,
            int productsPerPage,
            ProductSorting productSorting
            )
        {
            var productsQuery = this.data.Products.AsQueryable();

            ;

            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery
                    .Where(x => x.ProductsSubCategories.Any(x => x.SubCategory.Category.Name == category));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {

                productsQuery = productsQuery
                    .Include(x => x.ProductsSubCategories)
                    .ThenInclude(x => x.SubCategory)
                    .ThenInclude(x => x.Category)
                    .ToList()

                    .Where(x => x.ProductsSubCategories.Any(sb => sb.SubCategory.Name.ToLower().Contains(searchTerm.ToLower()))
                    || x.ProductsSubCategories.Any(sb => sb.SubCategory.Category.Name.ToLower().Contains(searchTerm.ToLower()))
                    || x.Title.ToLower().Contains(searchTerm.ToLower())
                    || (x.Title + " " + x.ProductCondition.ToString()).ToLower().Contains(searchTerm.ToLower())
                    || x.Description.ToLower().Contains(searchTerm.ToLower())).AsQueryable();

            }

            productsQuery = productSorting switch
            {


                ProductSorting.NameAlphabetically => productsQuery = productsQuery.OrderBy(x => x.Title),
                ProductSorting.NameDescending => productsQuery = productsQuery.OrderByDescending(x => x.Title),
                ProductSorting.PriceAscending => productsQuery = productsQuery.OrderBy(x => x.Price),
                ProductSorting.PriceDescending => productsQuery = productsQuery.OrderByDescending(x => x.Price),
                ProductSorting.Newest or _ => productsQuery.OrderByDescending(x => x.PublishedOn)

                //TODO : After implementation of Rating implement sorting by rating criteria too

            };

            var totalProducts = productsQuery.Count();

            var products = productsQuery
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage)
                .Select(x => new ProductServiceModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Condition = x.ProductCondition.ToString()
                })
                .ToList();

            var productCategories = this.data.Categories.Select(x => x.Name).Distinct().ToList();

            return new ProductQueryServiceModel
            {
                Products = products,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                TotalProducts=totalProducts
            };
        }

        public IEnumerable<string> AllProductCategories()
        => this.data
            .Categories
            .Select(x => x.Name)
            .Distinct()
            .OrderBy(ca=>ca)
            .ToList();
    }
}
