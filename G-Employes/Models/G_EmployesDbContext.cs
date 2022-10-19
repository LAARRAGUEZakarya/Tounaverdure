using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using G_Employes.Areas.Identity.Data;
using G_Employes.Models;
using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using GestionEmployes.Models.repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace G_Employes.Data
{
    public class G_EmployesDbContext : IdentityDbContext<G_EmployesUser>
    {
        public G_EmployesDbContext(DbContextOptions<G_EmployesDbContext> options)
            : base(options)
        {
        }
        public DbSet<Overier> overiers { get; set; }
        public DbSet<CategorieProduit> categorieProduits { get; set; }
        public DbSet<Produit> produits { get; set; }
        public DbSet<Operations> operations { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<DetailsPointeuse> detailsPointeuses { get; set; }
        public DbSet<Equipe> equipe { get; set; }
        public DbSet<Projet> projet { get; set; }
        public DbSet<Categorie> categorie { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
