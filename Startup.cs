using System;
using System.Globalization;
using IJustWatched.Data;
using IJustWatched.Hubs;
using IJustWatched.Models;
using IJustWatched.Models.CustomConstraits;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Westwind.AspNetCore.Markdown;


namespace IJustWatched
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IJustWatchedContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("IJustWatchedContext")));
            
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IJustWatchedContext>();
            
            services.AddMarkdown();
            
            services.AddSignalR();
            
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddDataAnnotationsLocalization()
                .AddViewLocalization()
                .AddApplicationPart(typeof(MarkdownPageProcessorMiddleware).Assembly);
            
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };
 
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            
            // add here your route constraint   
            services.Configure<RouteOptions>(routeOptions =>   
            {  
                routeOptions.ConstraintMap.Add("userIdConstraint", typeof(UserIdConstraint));  
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMarkdown();
            
            app.UseSignalR(routes =>
            {
                routes.MapHub<CommentHub>("/comments");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Home", template: "/",
                    new {controller="Home", action = "Index"});
                
                routes.MapRoute(
                    name: "Feed", template: "feeds/",
                    new {controller="Home", action = "Index"});
                
                routes.MapRoute(
                    name: "Films", template: "films/",
                    new {controller="Films", action = "Index"});

                routes.MapRoute(
                    name: "Film", template: "film/{filmId:int}", 
                    defaults: new {controller = "Film", action = "Index"});
                
                routes.MapRoute(
                    name: "Account", template: "acc/{action=Login}",
                    new {controller="Account"});
                
                routes.MapRoute(
                    name: "Review", template: "review/{reviewId:int}",
                    new {controller="Review", action="Index"});
                
                routes.MapRoute(
                    name: "NewReview", template: "newReview/{filmTitle?}",
                    new {controller="Review", action = "New"});
                
                routes.MapRoute(
                    name: "Roles", template: "roles/{action=Index}",
                    new {controller="Roles"});
                
                routes.MapRoute(
                    name: "Profile", template: "user/{userId:length(36)}",
                    new {controller="User", action="Index"});
                
                routes.MapRoute(
                    name: "MyProfile", template: "profile/",
                    new {controller="User", action="Index" });
                
                routes.MapRoute(
                    name: "Subscribe", template: "subscribe/{onUserId:length(36)}",
                    new {controller="User", action="Subscribe" });
                
                routes.MapRoute(
                    name: "Unsubscribe", template: "unsubscribe/{onUserId:length(36)}",
                    new {controller="User", action="Unsubscribe" });
                
                routes.MapRoute(
                    name: "DeleteComment", template: "deleteComment/{id:int}",
                    new {controller="Review", action="DeleteComment" });
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
            });
        }
    }
}
