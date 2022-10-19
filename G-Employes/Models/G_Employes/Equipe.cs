using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionEmployes.Models.repositories
{
    [Index(nameof(Nom_equipe), IsUnique = true)]
    public class Equipe
    {
        public int Id { get; set; }

        [Required]
        public string Nom_equipe { get; set; }

        public virtual Projet Projet { get; set; }

        
        public virtual ICollection<Overier> Employes{ get; set; }

    }
}
