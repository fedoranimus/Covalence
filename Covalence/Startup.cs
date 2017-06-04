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
using Microsoft.Data.Sqlite;
using System.Threading;
using OpenIddict.Core;
using OpenIddict.Models;
using AspNet.Security.OAuth.Extensions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable CORS
            services.AddCors();

            // Add framework services.
            services.AddMvc();

            ConfigureDatabase(services);            

            // Register the Identity services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
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

                // Enable the authorization, logout, userinfo, and introspection endpoints.
                // options.EnableAuthorizationEndpoint("/connect/authorize")
                //        .EnableLogoutEndpoint("/connect/logout")
                //        .EnableIntrospectionEndpoint("/connect/introspect")
                //        .EnableUserinfoEndpoint("/api/userinfo");

                // Allow client applications to use the grant_type=password flow.
                options.AllowPasswordFlow();
                //options.AllowImplicitFlow();
                options.AllowRefreshTokenFlow();

                // Return a JWT rather than a traditional token
                options.UseJsonWebTokens();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                options.AddEphemeralSigningKey();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.TryAddSingleton(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Audience = "http://localhost:5000",
                Authority = "http://localhost:5000",
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Subject,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role,
                }
            });

            services.AddScoped<ITagService, TagService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //TODO: Only see when in development!!!
            //if(_env.IsDevelopment())
            Seed(app, context);

            // If you prefer using JWT, don't forget to disable the automatic
            // JWT -> WS-Federation claims mapping used by the JWT middleware:
            //


            var jwtOptions = app.ApplicationServices.GetService<JwtBearerOptions>();
            app.UseJwtBearerAuthentication(jwtOptions);

            app.UseCors(builder => 
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin()
            );

            //app.UseIdentity();

            app.UseOpenIddict();

            if (env.IsDevelopment())
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
        public virtual async void Seed(IApplicationBuilder app, ApplicationDbContext context) {
            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            
            context.Database.Migrate();

            if(!context.Tags.Any()) {
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

            var seedUser = new ApplicationUser() {
                Email = "fixture@test.com",
                UserName = "fixture@test.com",
                FirstName = "Fixture",
                LastName = "Test",
                Location = "03062",
                EmailConfirmed = true
            };

            if(!context.Users.Any()) {
                var result = await userManager.CreateAsync(seedUser, "123Abc!");
                if(result.Succeeded) {
                    Console.WriteLine("Added User");
                }
            }

            context.SaveChanges();
        }

        public virtual void ConfigureDatabase(IServiceCollection services) {
            var connectionString = @"User Id=postgres;Password=@45jJq#2FJdw;Host=192.168.1.16;Port=5432;Database=covalence";

            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql(connectionString);
                options.UseOpenIddict();
            });
        }
    }
}
