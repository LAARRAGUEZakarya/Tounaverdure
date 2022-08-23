using G_Employes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.Repositories.R_Emplyes
{
    public class CategorieOverierDbRepository : IGestionEmployes<CategorieOverier>
    {
        G_EmployesDbContext db;
        public CategorieOverierDbRepository(G_EmployesDbContext _db)
        {

            db = _db;
        }

        public void Add(CategorieOverier categorie)
        {
            db.categorieOveriers.Add(categorie);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var categorie = Find(id);
            db.categorieOveriers.Remove(categorie);
            db.SaveChanges();
        }

        public CategorieOverier Find(int id)
        {
            var categorie = db.categorieOveriers.Find(id);
            return categorie;
        }

        public IList<CategorieOverier> List()
        {
            return db.categorieOveriers.ToList();
        }

        public void Update(int id, CategorieOverier Newcategorie)
        {
            db.categorieOveriers.Update(Newcategorie);
            db.SaveChanges();

        }
    }
}
