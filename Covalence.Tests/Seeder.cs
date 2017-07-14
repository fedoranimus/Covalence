using System;
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
            // if(!context.Database.EnsureCreated())
            //     context.Database.Migrate();

            SeedTags(context);
            //SeedPost(context);

            context.SaveChanges();
        }

        private static void SeedTags(ApplicationDbContext context)
        {
            context.Tags.Add(new Tag() {
                    Name = "Physics"
            });

            context.Tags.Add(new Tag() {
                Name = "Chemistry"
            });

            context.Tags.Add(new Tag() {
                Name = "Biology"
            });
        }

        private static void SeedPost(ApplicationDbContext context)
        {
            context.Posts.Add(new Post(){
                Category = PostType.Mentor,
                Content = "Seeded Content",
                Title = "Seeded Post",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Author = new ApplicationUser()
            });
        }
    }
}