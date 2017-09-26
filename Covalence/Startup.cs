using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Covalence.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using OpenIddict.Core;
using OpenIddict.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Data.Sqlite;
using Covalence.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.HttpOverrides;

namespace Covalence
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if(env.IsDevelopment())
                builder.AddUserSecrets<Startup>();
            
            Configuration = builder.Build();

            _env = env;
        }

        private IHostingEnvironment _env { get; set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable CORS
            services.AddCors();

            services.AddTransient<IEmailSender, EmailSender>();

            // Add framework services.
            services.AddMvc();

            if(!_env.IsStaging())
                services.Configure<AuthMessageSenderOptions>(Configuration);

            ConfigureDatabase(services, _env);            

            // Register the Identity services.
            services.AddIdentity<ApplicationUser, IdentityRole>(config => {
                    config.SignIn.RequireConfirmedEmail = _env.IsProduction();
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Register the OpenIddict services, including the default Entity Framework stores.
            services.AddOpenIddict(options => 
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<ApplicationDbContext>();
                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the token endpoint (required to use the password flow).
                options.EnableTokenEndpoint("/connect/token");

                // Allow client applications to use the grant_type=password flow.
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();

                // Return a JWT rather than a traditional token
                //options.UseJsonWebTokens();

                // During development, you can disable the HTTPS requirement.
                //if(!_env.IsProduction())
                options.DisableHttpsRequirement();

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                //options.AddEphemeralSigningKey();
            });

            services.AddAuthentication()
                    .AddOAuthValidation();

            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IPostService, PostService>();
            
            services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, ApplicationDbContext context, IPostService postService, ITagService tagService, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            context.Database.Migrate();

            app.UseCors(builder => 
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin()
            );

            if(_env.IsDevelopment() || _env.IsStaging()) 
            {
                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                Seed(userManager, context, postService, tagService);
            }

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            
        }
        public virtual async void Seed(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IPostService postService, ITagService tagService) {

            var physicsTag = new Tag() {
                    Name = "Physics"
                };

            var chemistryTag = new Tag() {
                    Name = "Chemistry"
                };

            var biologyTag = new Tag() {
                    Name = "Biology"
                };

            if(!context.Tags.Any()) {
                context.Tags.Add(physicsTag);
                context.Tags.Add(chemistryTag);
                context.Tags.Add(biologyTag);
            }

            var seedUser = new ApplicationUser() {
                Email = "fixture@test.com",
                UserName = "fixture@test.com",
                FirstName = "Fixture",
                LastName = "Test",
                Location = "03062",
                EmailConfirmed = true
            }; 

            var genji = new ApplicationUser() {
                Email = "genji@overwatch.com",
                UserName = "genji@overwatch.com",
                FirstName = "Genji",
                LastName = "Shimada",
                Location = "44600",
                EmailConfirmed = true
            };

            var mccree = new ApplicationUser() {
                Email = "mccree@overwatch.com",
                UserName = "mccree@overwatch.com",
                FirstName = "Jesse",
                LastName = "McCree",
                Location = "87501",
                EmailConfirmed = true
            };

            if(!context.Users.Any()) {
                var user = await userManager.CreateAsync(seedUser, "123Abc!");
                if(user.Succeeded) {
                    Console.WriteLine("User Added");
                }

                var genjiResult = await userManager.CreateAsync(genji, "123Abc!");
                if(genjiResult.Succeeded) {
                    Console.WriteLine("Genji Added");
                }

                var mccreeResult = await userManager.CreateAsync(mccree, "123Abc!");
                if(mccreeResult.Succeeded) {
                    Console.WriteLine("McCree Added");
                }
            }

            await context.SaveChangesAsync();

            await tagService.AddTag(physicsTag, genji);
            await tagService.AddTag(biologyTag, mccree);

            var post1 = new PostViewModel(){
                Title = "Test Post 1",
                Content = @"_Lorem ipsum dolor sit amet_, **consectetur** adipiscing elit. $\sqrt{3x-1}+(1+x)^2$ 
                Curabitur lectus ipsum, posuere sed ultrices ac, molestie vel lorem. Duis consectetur, nunc in finibus dictum, sapien nunc scelerisque purus, ut elementum metus metus ac odio. Sed augue purus, posuere in dui non, vulputate iaculis leo. Duis at convallis dolor. Proin porttitor odio vitae fringilla mollis. In eget molestie arcu. Aliquam luctus nisi et lorem imperdiet euismod. Integer lorem lectus, aliquet non ante vel, venenatis rutrum velit. Nunc iaculis venenatis laoreet. Curabitur pharetra non felis ut cursus. Aliquam ut laoreet leo. Mauris facilisis, nibh sed pellentesque vulputate, ex arcu sodales felis, vitae tincidunt sem nibh ut elit. Donec ac finibus risus. Duis feugiat tellus facilisis risus suscipit, elementum varius ex efficitur. Praesent eu varius lectus, id sodales est. Nunc tincidunt purus eget neque imperdiet porttitor.",
                Tags = new List<string>() { "physics", "biology" },
                Category = (int)PostType.Mentor
            };

            await postService.CreatePost(genji, post1);

            await context.SaveChangesAsync();
        }

        public virtual void ConfigureDatabase(IServiceCollection services, IHostingEnvironment env) {

            if(env.IsDevelopment()) 
            {
                var db = new SqliteConnection("DataSource=:memory:");
                db.Open();

                services.AddDbContext<ApplicationDbContext>( options => {
                    options.UseSqlite(db);
                    options.UseOpenIddict();
                });
            } 
            else
            {
                var connectionString = @"User Id=postgres;Password=@45jJq#2FJdw;Host=192.168.1.16;Port=5432;Database=covalence"; // TODO: Regenerate password and store in env var

                services.AddDbContext<ApplicationDbContext>(options => {
                    options.UseNpgsql(connectionString);
                    options.UseOpenIddict();
                });
            }
        }
    }
}
