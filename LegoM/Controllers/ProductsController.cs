namespace LegoM.Controllers
{
    using AutoMapper;
    using LegoM.Areas.Admin;
    using LegoM.Data;
    using LegoM.Infrastructure;
    using LegoM.Models.Products;
    using LegoM.Services.Products;
    using LegoM.Services.Questions;
    using LegoM.Services.Reviews;
    using LegoM.Services.Traders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static WebConstants;

    public class ProductsController:Controller
    {
        private readonly IProductsService products;
        private readonly ITraderService traders;
        private readonly IReviewService reviews;
        private readonly IQuestionsService questions;
        private readonly IMapper mapper;
        private readonly LegoMDbContext data;

        public ProductsController(LegoMDbContext data, IProductsService products, ITraderService traders, IMapper mapper, IReviewService reviews, IQuestionsService questions)
        {
            this.data = data;
            this.products = products;
            this.traders = traders;
            this.mapper = mapper;
            this.reviews = reviews;
            this.questions = questions;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.traders.IsUserTrader(this.User.Id()) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(TradersController.Become), "Traders");
            }

            return View(new ProductFormModel
            {
                Categories = this.products.AllCategories(),
                SubCategories = this.products.AllSubCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(ProductFormModel product)
        {
            string merchantId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!product.AgreeOnTermsOfPolitics && !isUserAdmin)
            {
                this.ModelState.AddModelError(nameof(product.AgreeOnTermsOfPolitics), "You must agree before submiting.");
            }
            if (!this.products.CategoryExists(product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exists.");
            }
             if (!this.products.SubCategoryExists(product.SubCategoryId,product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.SubCategoryId), "SubCategory is not valid.");
            }

            if (!ModelState.IsValid)
            {

                product.Categories = this.products.AllCategories();
                product.SubCategories = this.products.AllSubCategories();

                return View(product);
            }

            var productId = this.products
                   .Create(product.Title,
                           product.Description,
                           product.FirstImageUrl,
                           product.SecondImageUrl,
                           product.ThirdImageUr,
                           product.Price,
                           product.Quantity,
                           product.CategoryId,
                           product.SubCategoryId,
                           product.Condition.Value,
                           product.Delivery.Value,
                           merchantId,
                           isUserAdmin);

            TempData[GlobalMessageKey] = $"Your product was added { (isUserAdmin ? string.Empty : "and is awaiting for approval!") }";

            return RedirectToAction(nameof(Details),new { id= productId});

        }

        public IActionResult All([FromQuery]ProductsQueryModel query)
        {
            if (!this.products.SubCategoryIsValid(query.SubCategory,query.Category))
            {
                return BadRequest();
            }
            
            var queryResult = this.products.All(
            query.Category,
            query.SubCategory,
            query.SearchTerm,
            query.CurrentPage,
            ProductsQueryModel.ProductsPerPage,
            query.ProductSorting);

            var categories = this.products.AllCategories();
            var subCategories = this.products.AllSubCategories();

            query.Products = queryResult.Products;
            query.Categories = categories;
            query.SubCategories = subCategories;
            query.TotalProducts = queryResult.TotalProducts;
                  
            return this.View(query);
        }


        [Authorize]
        public IActionResult Mine()
        {
            var myProducts=this.products.ByUser(this.User.Id());

            if (this.User.IsAdmin())
            {
                return RedirectToAction(nameof(All)); 
            }

            return this.View(myProducts);
        }

        [Authorize]
        public IActionResult Edit(string Id)
        {
            string merchantId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.products.ProductExists(Id))
            {
                return NotFound();
            }

            if (!this.products.ProductIsByTrader(Id, merchantId) && !isUserAdmin)
            {
                return BadRequest();
            }

            var product = this.products.Details(Id);

            var productForm = this.mapper.Map<ProductFormModel>(product);

            productForm.Categories = this.products.AllCategories();
            productForm.SubCategories = this.products.AllSubCategories();

            return View(productForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string Id,ProductFormModel product)
        {
            string merchantId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null&& !isUserAdmin)
            {
                return BadRequest();
            }        

            if (!this.products.ProductIsByTrader(Id, merchantId) && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.products.ProductExists(Id))
            {
                return NotFound();
            }

            if (!this.products.CategoryExists(product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exists.");
            }
            if (!this.products.SubCategoryExists(product.SubCategoryId, product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.SubCategoryId), "SubCategory is not valid.");
            }

            if (!ModelState.IsValid)
            {

                product.Categories = this.products.AllCategories();
                product.SubCategories = this.products.AllSubCategories();

                return View(product);
            }                
           
            this.products.Edit(
                  Id,
                  product.Title,
                  product.Description,
                  product.FirstImageUrl,
                  product.SecondImageUrl,
                  product.ThirdImageUr,
                  product.Price,
                  product.Quantity,
                  product.CategoryId,
                  product.SubCategoryId,
                  product.Condition.Value,
                  product.Delivery.Value,
                  merchantId,
                  isUserAdmin);
           
            TempData[GlobalMessageKey] = $"Your product was edited { (isUserAdmin ? string.Empty : "and is awaiting for approval!") } ";

            return RedirectToAction(nameof(Details), new { Id });
        }


        public IActionResult Details(string id,[FromQuery]ProductsDetailsQueryModel query)
        {
            string merchantId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            var product = this.products.Details(id);

            if (product == null)
            {
                return NotFound();
            }

            if (!this.products.ProductIsByTrader(id,merchantId)&&
                !product.IsPublic&&
                !isUserAdmin)
            {
                return BadRequest();
            }

            var similarProducts = this.products.GetSimilarProducts(id);

            var reviewsQueryResult = this.reviews.All(
                query.ReviewsSearchTerm,
                query.ReviewsCurrentPage,
                ProductsDetailsQueryModel.ReviewsPerPage,
                query.ReviewFiltering,
                productId: id);

            var questionsQueryResult = this.questions.All(
                currentPage: query.QuestionsCurrentPage,
                questionsPerPage: ProductsDetailsQueryModel.QuestionsPerPage,
                productId: id);

            var reviewsStatistics = this.reviews.GetStatisticsForProduct(id);
           
            query.Product = product;
            query.SimilarProducts = similarProducts;
            query.ProductReviewsStatistics = reviewsStatistics;

            query.Reviews = reviewsQueryResult.Reviews;
            query.TotalReviews = reviewsQueryResult.TotalReviews;

            query.Questions = questionsQueryResult.Questions;
            query.TotalQuestions = questionsQueryResult.TotalQuestions;
            
            return this.View(query);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            string merchantId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.products.ProductExists(id))
            {
                return NotFound();
            }

            if (!this.products.ProductIsByTrader(id, merchantId) && !isUserAdmin)
            {
                return BadRequest();
            }

            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(string id,ProductDeleteFormModel productDelete)
        {
            string merchantId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.products.ProductIsByTrader(id, merchantId) && !isUserAdmin)
            {
                return BadRequest();
            }

            if (productDelete.SureToDelete)
            { 
                var deleted = this.products.DeleteProduct(id, isUserAdmin);

                if (!deleted)
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction(nameof(Details),new { id});
            }                 
            
            this.TempData[GlobalMessageKey] = "Your product was deleted!";

            if (isUserAdmin)
            {
                return RedirectToAction(nameof(Areas.Admin.Controllers.ProductsController.Existing),new { area=AdminConstants.AreaName });
            }

            return RedirectToAction(nameof(All));
        }

    }
}
