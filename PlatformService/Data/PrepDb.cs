using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder appBuilder, bool isProd)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                try
                {
                    System.Console.WriteLine("--> Attempting to run migrations in Prod");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"--> Could not run migration - {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                System.Console.WriteLine("Seeding data....");

                context.Platforms.AddRange(
                    new Platform { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing FOundation", Cost = "Free" }
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