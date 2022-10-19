using GestionEmployes.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.ViewModel
{
    public class CategorieProduitViewModel
    {
        public int produitId { get; set; }
        public string Desgination { get; set; }
        public float Prix { get; set; }
        public int Quantite { get; set; }
        public string typeUnite { get; set; }
        public int QttUpdate { get; set; }
        public int CategorieId { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile File { get; set; }
        public List<CategorieProduit> Categories { get; set; }
    }
}
