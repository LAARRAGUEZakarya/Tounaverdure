using G_Employes.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.Repositories.R_Stock
{
    public class OperationsDbRepository : IGestionEmployes<Operations>
    {
        G_EmployesDbContext db;
        public OperationsDbRepository(G_EmployesDbContext _db)
        {
            db = _db;
        }
        public void Add(Operations entity)
        {
            db.operations.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
          
            foreach(var item in db.operations)
            {
                db.operations.Remove(item);
            }
            db.SaveChanges();
        }

        public Operations Find(int id)
        {
            var operations = db.operations.Include(o=>o.ovrier).Include(r=>r.produit).SingleOrDefault(p=>p.Id==id);
            return operations;
        }

        public IList<Operations> List()
        {
            return db.operations.Include(o => o.ovrier).Include(r => r.produit).ToList();
        }

        public void Update(int id, Operations entity)
        {
            db.operations.Update(entity);
        }
        List<Overier> chefs;
        public IList<Overier> ListChef()
        {

            foreach (var item in db.overiers)
            {
                if (item.Type == "chef")
                {
                    chefs.Add(item);
                }
            }
            return chefs;
        }
    }
}
