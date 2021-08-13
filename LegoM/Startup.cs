namespace LegoM
{
    using LegoM.Controllers;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Infrastructure;
    using LegoM.Services.Answers;
    using LegoM.Services.Comments;
    using LegoM.Services.Favourites;
    using LegoM.Services.Merchants;
    using LegoM.Services.Products;
    using LegoM.Services.Questions;
    using LegoM.Services.Reviews;
    using LegoM.Services.Statistics;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
  

    public class Startup
    {
        public Startup(IConfiguration configuration)
        => Configuration = configuration;
        

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<LegoMDbContext>(options => options
                  .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                  {
                      options.Password.RequireDigit = false;
                      options.Password.RequireLowercase = false;
                      options.Password.RequireNonAlphanumeric = false;
                      options.Password.RequireUppercase = false;

                  })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LegoMDbContext>();

            services
                .AddControllersWithViews(options =>
                {

                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();

                });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddAutoMapper(typeof(Startup));
            services.AddMemoryCache();

            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IMerchantService, MerchantService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFavouriteService, FavouriteService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
            {



                endpoints.MapDefaultAreaRoute();

                endpoints.MapReviewsDetailsRoute();
                endpoints.MapQuestionsDetailsRoute();

                endpoints.MapAnswersAddRoute();
                endpoints.MapCommentsAddRoute();
               

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
