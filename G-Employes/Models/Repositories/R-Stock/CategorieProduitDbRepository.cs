using G_Employes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.Repositories.R_Stock
{
    public class CategorieProduitDbRepository : IGestionEmployes<CategorieProduit>
    {

        G_EmployesDbContext db;
        public CategorieProduitDbRepository(G_EmployesDbContext _db)
        {
            db = _db;
        }
        public void Add(CategorieProduit entity)
        {
            db.categorieProduits.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var categorie = Find(id);
            db.categorieProduits.Remove(categorie);
            db.SaveChanges();
        }

        public CategorieProduit Find(int id)
        {
            var categorie = db.categorieProduits.SingleOrDefault(c => c.Id == id);
            return categorie;
        }

        public IList<CategorieProduit> List()
        {
            return db.categorieProduits.ToList();
        }

        public void Update(int id, CategorieProduit entity)
        {
            db.categorieProduits.Update(entity);
            db.SaveChanges();
        }
    }
}

