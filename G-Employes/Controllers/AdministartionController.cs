using G_Employes.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_Employes.Controllers
{
    public class AdministartionController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public AdministartionController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }


        [HttpGet]
        public  ActionResult CreateRole()
        {
    
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(AdministrationCreateRoleViewModel model)
        {
            IdentityRole role = new IdentityRole
            {
                Name = model.RoleName
            };
            await roleManager.CreateAsync(role);
            return View(model);
        }
    }
}
