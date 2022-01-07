using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                System.Console.WriteLine("Seeding data....");

                context.Platforms.AddRange(
                    new Platform { Name = "Dotnet", Publisher = "Microsoft", Cost="Free" },
                    new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost="Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing FOundation", Cost="Free" }
                );

                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Seed data exists....");
            }
        }
    }
}