using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class DetailsPointeuse
    {
        public int Id { get; set; }
        public int IdEmploye { get; set; }
        
        public string Name { get; set; }
        public DateTime dateWorkCheck { get; set; }
        public string Status { get; set; }
        public TimeSpan NbrHoursParJour { get; set; }
        public TimeSpan NbrHoursParMois { get; set; }
        public bool Etat { get; set; }

        public TimeSpan TimeOfIN { get; set; }
        public TimeSpan TimeOfOut { get; set; }

        public string TimeOfInAugDid { get; set; }
        public string TimeOfOutAugDid { get; set; }
        public string SumTimeOfInAugDid { get; set; }
        public string SumTimeOfOutAugDid { get; set; }
        public string etatTimeofin { get; set; }

        public float  SalaireParMois { get; set; }
    }
}
