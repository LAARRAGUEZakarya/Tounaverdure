using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class Operations
    {
        public int Id { get; set; }
        public Produit produit { get; set; }
        public Overier ovrier { get; set; }

        public int Qtt_Diduir { get; set; }
        public DateTime Date_operation  { get; set; }
    }
}
