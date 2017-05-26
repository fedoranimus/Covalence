using Covalence.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

namespace Covalence.Tests {
    public class TestStartup : Startup  
    {
        public TestStartup(IHostingEnvironment env): base(env) {

        }
        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>( options => {
                options.UseInMemoryDatabase();
                options.UseOpenIddict();
            });
        }

        public override void Seed(IApplicationBuilder app) {
            app.Seed();
        }
    }
}