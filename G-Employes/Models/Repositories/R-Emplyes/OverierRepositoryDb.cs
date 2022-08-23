using G_Employes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.Repositories.R_Emplyes
{
    public class OverierRepositoryDb : IGestionEmployes<Overier>
    {

        G_EmployesDbContext db;
        public OverierRepositoryDb(G_EmployesDbContext _db)
        {
            db = _db;
        }
        public void Add(Overier entity)
        {
            db.overiers.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var overier = Find(id);
            db.overiers.Remove(overier);
            db.SaveChanges();
        }

        public Overier Find(int id)
        {
            var overier = db.overiers.Find(id);
            return overier;
        }

        public IList<Overier> List()
        {
            return db.overiers.ToList();
        }

        public void Update(int id, Overier entity)
        {
            db.overiers.Update( entity);
            db.SaveChanges();
        }
    
        
    }
}
