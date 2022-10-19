using G_Employes.Areas.Identity.Data;
using GestionEmployes.Models;
using GestionEmployes.Models.repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.ViewModel
{
    public class CategorieOverierViewModel
    {
        public int OverierId { get; set; }
        public string Name { get; set; }
        public string Prenom { get; set; }
        public string Adress { get; set; }
        public string Tel { get; set; }
        public DateTime Date_embauche { get; set; }
        public string CIN { get; set; }
        public string Sexe { get; set; }
        public string Type { get; set; }
        public IFormFile File { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public G_EmployesUser user { get; set; }
        public int IdCategorie { get; set; }

        public List <Categorie> categories { get; set; }
    }
}
