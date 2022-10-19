using System;
using G_Employes.Areas.Identity.Data;
using G_Employes.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(G_Employes.Areas.Identity.IdentityHostingStartup))]
namespace G_Employes.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<G_EmployesDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("G_EmployesDbContextConnection")));

                services.AddDefaultIdentity<G_EmployesUser>(options => 
                {
                    
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequireDigit = false;
                   
                }).AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<G_EmployesDbContext>();

                
            });
        }
    }
}