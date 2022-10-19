using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionEmployes.Models.repositories
{
    [Index(nameof(Fonctionnalite), IsUnique = true)]
    public class Categorie
    {
        public int Id { get; set; } 
        public string Fonctionnalite { get; set; }
        public virtual ICollection<Overier> Employes { get; set; }



    }
}
