using G_Employes.Data;
using GestionEmployes.Models.G_Stock;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.Repositories.R_Stock
{
    public class CommandeDbRepository : IGestionEmployes<Commande>
    {
        G_EmployesDbContext db;
        public CommandeDbRepository(G_EmployesDbContext _db)
        {
            db = _db;
        }
        public void Add(Commande entity)
        {
            db.Commandes.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {

            var item = Find(id);
                db.Commandes.Remove(item);
           
            db.SaveChanges();
        }

        public Commande Find(int id)
        {
            var commande = db.Commandes.Include(o=>o.ovrier).Include(r=>r.produit).SingleOrDefault(p=>p.Id==id);
            return commande;
        }

        public IList<Commande> List()
        {
            return db.Commandes.Include(o => o.ovrier).Include(r => r.produit).Include(o => o.produit.Categorie).ToList();
        }

        public void Update(int id, Commande entity)
        {
            db.Commandes.Update(entity);
            db.SaveChanges();
        }

    }
}
