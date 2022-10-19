using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public class Operations
    {
        public int Id { get; set; }
      
       

        public int Qtt_Diduir { get; set; }
        public int Qtt_Augmenter { get; set; }
        public string Status { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date_operation  { get; set; }

        public string FullNameChef { get; set; }
        public string CIN { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Disgination { get; set; }
        public string typeUnite { get; set; }
        public int Qtt { get; set; }
        public float Prix { get; set; }
        public string imageUrl { get; set; }

    }
}
