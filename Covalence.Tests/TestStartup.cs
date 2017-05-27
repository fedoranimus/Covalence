using Covalence.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;

namespace Covalence.Tests {
    public class TestStartup : Startup  
    {
        public TestStartup(IHostingEnvironment env): base(env) {

        }
        public override void ConfigureDatabase(IServiceCollection services)
        {
            var db = new SqliteConnection("DataSource=:memory:");
            db.Open();

            services.AddDbContext<ApplicationDbContext>( options => {
                options.UseSqlite(db);
                options.UseOpenIddict();
            });
        }

        public override void Seed(IApplicationBuilder app, ApplicationDbContext context) {
            app.Seed(context);
        }
    }
}