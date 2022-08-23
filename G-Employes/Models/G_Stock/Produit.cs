using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class Produit
    {
        public int Id { get; set; }
        public string Desgination { get; set; }
        public float Prix { get; set; }
        public int Quantite { get; set; }
        public string ImageUrl { get; set; }
        public CategorieProduit Categorie { get; set; }
    }
}
