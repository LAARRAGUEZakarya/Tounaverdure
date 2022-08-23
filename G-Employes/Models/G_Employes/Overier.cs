using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class Overier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prenom { get; set; }
        public string Adress { get; set; }
        public string Tel { get; set; }
        public DateTime Date_embauche { get; set; }
        public string CIN { get; set; }
        public string Sexe { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public CategorieOverier categorie { get; set; }

    }
}
