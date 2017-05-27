using Covalence;
using Covalence.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Covalence.Tests {
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app, ApplicationDbContext context)
        {
            if(!context.Database.EnsureCreated())
                context.Database.Migrate();

            SeedTags(context);

            context.SaveChanges();
        }

        private static void SeedTags(ApplicationDbContext context)
        {
            context.Tags.Add(new Tag() {
                    Name = "Physics",
                    Description = "Study of Motion"
            });

            context.Tags.Add(new Tag() {
                Name = "Chemistry",
                Description = "Study of Matter"
            });

            context.Tags.Add(new Tag() {
                Name = "Biology",
                Description = "Study of Life"
            });
        }
    }
}