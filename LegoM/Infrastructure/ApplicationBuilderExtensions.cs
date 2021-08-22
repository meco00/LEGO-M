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

    using static Areas.Admin.AdminConstants;

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
            SeedUser(services);

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
                    Name="Games and toys",
                    SubCategories=new[]
                    {
                        new SubCategory()
                        {
                            Name="For girls"
                        },
                          new SubCategory()
                        {
                            Name="For boys"
                        },
                         new SubCategory()
                        {
                            Name="Lego"
                        },
                           new SubCategory()
                        {
                            Name="Plush toys"
                        },
                           new SubCategory()
                        {
                            Name="Cars, trucks and motorcycles"
                        },
                           new SubCategory()
                        {
                            Name="Puzzels"
                        }, 
                        new SubCategory()
                        {
                            Name="Playing cards"

                        },
                         new SubCategory()
                        {
                            Name="Pistols"

                        },


                    }
                },
                  new Category()
                {
                    Name="For the student",
                    SubCategories=new[]
                    {
                        new SubCategory()
                        {
                            Name="Notebooks"
                        },
                         new SubCategory()
                        {
                            Name="School backpacks and bags"
                        }, 
                        new SubCategory()
                        {
                            Name="Pencilcases"

                        }, 
                        new SubCategory()
                        {
                            Name="Painting"


                        }, 
                        new SubCategory()
                        {
                            Name="Writing tools"

                        },  
                        new SubCategory()
                        {
                            Name="Sketches and blocks"

                        }, 
                        new SubCategory()
                        {
                            Name="Folders and boxes"

                        },
                         new SubCategory()
                        {
                            Name="Bindings"

                        },
                           new SubCategory()
                        {
                            Name="Cardboards"

                        },

                    }
                },
                    new Category()
                {
                    Name="Sport",
                    SubCategories=new[]
                    {
                        new SubCategory()
                        {
                            Name="Football"
                        },
                         new SubCategory()
                        {
                            Name="Volleyball"
                        },
                           new SubCategory()
                        {
                            Name="Federball"
                        },
                           new SubCategory()
                        {
                            Name="Boxing"
                        },

                    }
                },
                    new Category()
                {
                    Name="Gifts",
                    SubCategories=new[]
                    {
                         new SubCategory()
                        {
                            Name="Jewelry"
                        },
                        new SubCategory()
                        {
                            Name="Purses and travel bags"
                        },
                         new SubCategory()
                        {
                            Name="Greeting cards and books"
                        },
                           new SubCategory()
                        {
                            Name="Piggy banks"
                        },
                           new SubCategory()
                        {
                            Name="Keychains"
                        },

                    }
                },
                    new Category()
                {
                    Name="Books",
                    SubCategories=new[]
                    {
                         new SubCategory()
                        {
                            Name="Books for painting"
                        },
                        new SubCategory()
                        {
                            Name="Globuses"
                        },
                         new SubCategory()
                        {
                            Name="Еncyclopedias"
                        },
                           new SubCategory()
                        {
                            Name="Children's literature"
                        },
                           new SubCategory()
                        {
                            Name="Literature for teenagers"
                        },

                    }
                },

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


        private static void SeedUser(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            Task
              .Run(async () =>
              {
                  const string userEmail = "test@user.com";

                  if (await userManager.FindByEmailAsync(userEmail) != null)
                  {
                      return;
                  }                 

                  const string userPassword = "test";

                  var user = new User
                  {
                      Email = userEmail,
                      UserName = userEmail,
                      FullName = "TestUser"

                  };

                  await userManager.CreateAsync(user, userPassword);


              })
              .GetAwaiter()
              .GetResult();

        }

    }
}
