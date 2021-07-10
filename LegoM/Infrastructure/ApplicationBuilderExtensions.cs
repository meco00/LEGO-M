namespace LegoM.Infrastructure
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();

           var data = scopedService.ServiceProvider.GetService<LegoMDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        private static void SeedCategories(LegoMDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category()
                {
                    Name="Игри и играчки",
                    SubCategories=new[]
                    {
                        new SubCategory()
                        {
                            Name="За момичета"
                        },
                         new SubCategory()
                        {
                            Name="Лего"
                        },
                           new SubCategory()
                        {
                            Name="Плюшени играчки"
                        },
                    }
                },
                  new Category()
                {
                    Name="За ученика",
                    SubCategories=new[]
                    {
                        new SubCategory()
                        {
                            Name="Тетрадки"
                        },
                         new SubCategory()
                        {
                            Name="Ученически раници и чанти"
                        },
                           new SubCategory()
                        {
                            Name="Други"
                        },
                    }
                }

            });

            data.SaveChanges();
        }
    }
}
