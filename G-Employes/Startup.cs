using G_Employes.Areas.Identity.Data;
using G_Employes.Data;
using G_Employes.Models.Repositories.R_Emplyes;
using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using GestionEmployes.Models.repositories;
using GestionEmployes.Models.Repositories.R_Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_Employes
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
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IGestionEmployes<DetailsPointeuse>, DetailsPointeuseDbRepository>();
            services.AddScoped<IGestionEmployes<CategorieProduit>, CategorieProduitDbRepository>();
            services.AddScoped<IGestionEmployes<Produit>, ProduitDbRepository>();
            services.AddScoped<IGestionEmployes<Operations>, OperationsDbRepository>();
            services.AddScoped<IGestionEmployes<Commande>, CommandeDbRepository>();



            //services.AddControllersWithViews().AddNToastNotifyNoty(new NToastNotify.NotyOptions()
            //{
            //    ProgressBar = true,
            //    Timeout = 10000,
            //    Theme = "mint"
            //});
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.Configure<CookiePolicyOptions>(option =>
            {
                option.CheckConsentNeeded = context => false;
                option.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<G_EmployesDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("G_EmployesDbContextConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
