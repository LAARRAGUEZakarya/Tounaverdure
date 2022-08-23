using GestionEmployes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.ViewModel
{
    public class OperationProduitOverierViewModel
    {
        public int IdOperation { get; set;}
        public int idProduit { get; set; }
        public int idOverier { get; set; }
        public IList<Produit> produits { get; set;}
        public IList<Overier> ovriers { get; set;}
        public int Qtt_Diduir { get; set;}
        public DateTime Date_operation { get; set;}
        

    }
}
