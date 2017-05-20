using Covalence.API.Tags;
using Covalence.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class Seeder
{
    public static void Seed(this IApplicationBuilder app)
    {
        var db = app.ApplicationServices.GetService<ApplicationDbContext>();

        SeedTags(db);

        db.SaveChanges();
    }

    private static void SeedTags(ApplicationDbContext db)
    {
        db.Tags.Add(new Tag() {
                Name = "Physics",
                Description = "Study of Motion"
        });

        db.Tags.Add(new Tag() {
            Name = "Chemistry",
            Description = "Study of Matter"
        });

        db.Tags.Add(new Tag() {
            Name = "Biology",
            Description = "Study of Life"
        });
    }
}