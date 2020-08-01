using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Anime_Studio.DataAccess.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.DataAccess.Data.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Anime_Studio.Utility;

namespace Anime_Studio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //services.AddDefaultIdentity<IdentityUser> does not have support for roles. this was the starter and I had to change it to what is below.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(configure =>
                {
                    configure.Password.RequireNonAlphanumeric = false;
                    
                })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IEmailSender, EmailSender>(); //this uses the EmailSender Class built in Utility to bypass the email registration error.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                
            });
            //facebook developers website --> Add Nuget Package for Facebook Authentication as well
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "453481951235162418";
                options.AppSecret = "849be27a4dd281ad2baddbbfac4e81af6e";
            });

            //console.developers.google --> Add Nuget Package for Google Authentication as well.
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "4546274366970-nlg05m7o551psvdv2kcaglevclmk10oj.apps.googleusercontent.com";
                options.ClientSecret = "84_wiMuYJQJ028aLJHfP1Q511p";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
