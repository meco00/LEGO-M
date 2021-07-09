namespace LegoM.Infrastructure
{
    using LegoM.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();

           var data = scopedService.ServiceProvider.GetService<LegoMDbContext>();

            data.Database.Migrate();
            return app;
        }
    }
}
