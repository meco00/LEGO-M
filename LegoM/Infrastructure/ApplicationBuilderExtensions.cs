namespace LegoM.Infrastructure
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static WebConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<LegoMDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<LegoMDbContext>();

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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var adminRole = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(adminRole);

                    const string adminEmail = "admin@gm.com";

                    const string adminPassword = "admin0021";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin"

                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, adminRole.Name);

                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
