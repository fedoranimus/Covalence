using System;
using System.Threading.Tasks;
using Covalence.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Covalence
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var tagService = services.GetRequiredService<ITagService>();
                    var connectionService = services.GetRequiredService<IConnectionService>();
                    var env = services.GetRequiredService<IHostingEnvironment>();
                    var locationService = services.GetRequiredService<ILocationService>();
                    await DbInitializer.InitializeAsync(context, userManager, tagService, connectionService, locationService, env);
                }
                catch(Exception ex) 
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while seeding the database.");
                }
            }
            
            await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
