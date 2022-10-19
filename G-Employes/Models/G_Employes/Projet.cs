using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionEmployes.Models.repositories
{
    [Index(nameof(Nom_projet), IsUnique = true)]
    public class Projet
    {
        public int Id { get; set; }
        [Required]
       
        public string Nom_projet { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date_debut { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date_fin { get; set; }
        public string Etat { get; set; }
        public virtual ICollection<Equipe> Equipes { get; set; }
        
    }
}
