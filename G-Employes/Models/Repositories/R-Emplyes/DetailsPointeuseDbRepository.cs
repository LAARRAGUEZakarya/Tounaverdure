using G_Employes.Data;
using GestionEmployes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_Employes.Models.Repositories.R_Emplyes
{
    public class DetailsPointeuseDbRepository : IGestionEmployes<DetailsPointeuse>
    {
        G_EmployesDbContext db;
        public DetailsPointeuseDbRepository(G_EmployesDbContext _db)
        {

            db = _db;
        }

     

        public void Add(DetailsPointeuse entity)
        {
            db.detailsPointeuses.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = Find(id);
            db.detailsPointeuses.Remove(entity);
            db.SaveChanges();
        }

        public DetailsPointeuse Find(int id)
        {
            var entity = db.detailsPointeuses.Find(id);
            return entity;
        }

        public IList<DetailsPointeuse> List()
        {
            return db.detailsPointeuses.ToList();
        }

        public void Update(int id, DetailsPointeuse entity)
        {
            db.detailsPointeuses.Update(entity);
            db.SaveChanges();

        }

        
    }
 
 }

