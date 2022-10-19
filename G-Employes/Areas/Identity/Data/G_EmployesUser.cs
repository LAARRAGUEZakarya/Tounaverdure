using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace G_Employes.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the G_EmployesUser class
    public class G_EmployesUser : IdentityUser
    {
        [PersonalData]
        public int IdEmploye { get; set; }



        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Nom { get; set; }


      [PersonalData]
      [Column(TypeName = "nvarchar(100)")]
       public string Prenom { get; set; }

        //[PersonalData]
        //[Column(TypeName = "nvarchar(20)")]
        //public string Role { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string ImageUrl { get; set; }
    }
}
