using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class Produit
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Saiser la desgination !")]
        public string Desgination { get; set; }

        public float Prix { get; set; }
        [Required]
        public string TypeUnite { get; set; }
        [Required]
        public int Quantite { get; set; }
        public string ImageUrl { get; set; }
        public CategorieProduit Categorie { get; set; }
    }
}
