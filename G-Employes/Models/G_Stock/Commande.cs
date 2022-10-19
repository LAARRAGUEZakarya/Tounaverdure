using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models.G_Stock
{
    public class Commande
    {
        public int Id { get; set; }
        public Produit produit { get; set; }
        public Overier ovrier { get; set; }

        public int Qtt_Diduir { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date_operation { get; set; }
       
        public string Etat { get; set; }
    }
}
