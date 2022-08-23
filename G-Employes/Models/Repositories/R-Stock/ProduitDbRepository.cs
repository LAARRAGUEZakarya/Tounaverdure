using G_Employes.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.Repositories.R_Stock
{
    public class ProduitDbRepository : IGestionEmployes<Produit>
    {
        G_EmployesDbContext db;
        public ProduitDbRepository(G_EmployesDbContext _db)
        {
            db = _db;
        }
        public void Add(Produit entity)
        {
            db.produits.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var produit = Find(id);
            db.produits.Remove(produit);
            db.SaveChanges();
        }

        public Produit Find(int id)
        {
            
            var produit = db.produits.Include(c=>c.Categorie).SingleOrDefault(p => p.Id == id);
            return produit;

        }

        public IList<Produit> List()
        {
            return db.produits.Include(c=>c.Categorie).ToList();
        }

        public void Update(int id, Produit entity)
        {
            db.produits.Update(entity);
            db.SaveChanges();
        }
    }
}

