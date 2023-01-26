using GestionEmployes.Models.repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class Overier
    {
        public int Id { get; set; }
        
        public string Nom { get; set; }
       
        public string Prenom { get; set; }
       
        public string Adress { get; set; }
        public string Matricule  { get; set; }


        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "numéro incorrect")]

        public string Tel { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date_embauche { get; set; }
        

        public string CIN { get; set; }
      
        public string Sexe { get; set; }
       
        public string Type { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
       
        public int Salaire { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        
        public virtual Equipe Equipe { get; set; }

        public virtual Categorie Categorie { get; set; }




        public string EmailOld { get; set; }
        public string typeOld { get; set; }

    }
}
