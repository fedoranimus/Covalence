using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Identity;

namespace Covalence.Tests {
    public class TestStartup : Startup  
    {
        public TestStartup(IHostingEnvironment env): base(env) {

        }

        public override void ConfigureDatabase(IServiceCollection services, IHostingEnvironment env)
        {
            var db = new SqliteConnection("DataSource=:memory:");
            db.Open();

            services.AddDbContext<ApplicationDbContext>( options => {
                options.UseSqlite(db);
                options.UseOpenIddict();
            });
        }
    }
}