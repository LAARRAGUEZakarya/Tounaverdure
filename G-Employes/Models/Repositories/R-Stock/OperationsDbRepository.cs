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

            var item = Find(id);
            db.operations.Remove(item);
            
         
            db.SaveChanges();
        }

        public Operations Find(int id)
        {
            var operations = db.operations.SingleOrDefault(p=>p.Id==id);
            return operations;
        }

        public IList<Operations> List()
        {
            return db.operations.ToList();
        }

        public void Update(int id, Operations entity)
        {
            db.operations.Update(entity);
        }
     
    }
}
