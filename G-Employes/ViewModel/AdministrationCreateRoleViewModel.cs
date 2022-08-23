using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace G_Employes.ViewModel
{
    public class AdministrationCreateRoleViewModel
    {
        [Required]
        [Display(Name ="Saiser noveau role")]
        public string RoleName { get; set; }
    }
}
