using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Covalence.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.Migrate();

            if(context.Users.Any() || context.Tags.Any())
            {
                return;
            }

            var users = new ApplicationUser[]
            {
                new ApplicationUser() {
                    Email = "mccree@overwatch.com",
                    UserName = "mccree@overwatch.com",
                    FirstName = "Jesse",
                    LastName = "McCree",
                    Location = "87501",
                    EmailConfirmed = true
                },
                new ApplicationUser() {
                    Email = "genji@overwatch.com",
                    UserName = "genji@overwatch.com",
                    FirstName = "Genji",
                    LastName = "Shimada",
                    Location = "44600",
                    EmailConfirmed = true
                },
                new ApplicationUser() {
                    Email = "tracer@overwatch.com",
                    UserName = "tracer@overwatch.com",
                    FirstName = "Lena",
                    LastName = "Oxton",
                    Location = "03062",
                    EmailConfirmed = true
                },
                new ApplicationUser() {
                    Email = "dva@overwatch.com",
                    UserName = "dva@overwatch.com",
                    FirstName = "Hana",
                    LastName = "Song",
                    Location = "03062",
                    EmailConfirmed = true
                },
                new ApplicationUser() {
                    Email = "hanzo@overwatch.com",
                    UserName = "hanzo@overwatch.com",
                    FirstName = "Hanzo",
                    LastName = "Shimada",
                    Location = "03062",
                    EmailConfirmed = true
                },
                new ApplicationUser() {
                    Email = "mei@overwatch.com",
                    UserName = "mei@overwatch.com",
                    FirstName = "Mei-Ling",
                    LastName = "Zhou",
                    Location = "03062",
                    EmailConfirmed = true
                }
            };

            foreach(ApplicationUser user in users)
            {
               await userManager.CreateAsync(user, "123Abc!");
            }

            await context.SaveChangesAsync();

            var tags = new Tag[]
            {
                new Tag { Name = "Biology" },
                new Tag { Name = "Physics" },
                new Tag { Name = "chemistry" }
            };

            foreach(Tag tag in tags)
            {
                await context.Tags.AddAsync(tag);
            }

            await context.SaveChangesAsync();
        }
    }
}