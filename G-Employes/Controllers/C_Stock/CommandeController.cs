using G_Employes.Areas.Identity.Data;
using G_Employes.Data;
using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using GestionEmployes.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Stock
{
  
    public class CommandeController : Controller
    {
        
        private readonly SignInManager<G_EmployesUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IGestionEmployes<Commande> commandeRepository;
        private readonly IGestionEmployes<Produit> produitRepository;
        private readonly UserManager<G_EmployesUser> userManager;
        private readonly G_EmployesDbContext gestionEmployeContext;
       

        public object CommandeProduitOverierViewModel { get; private set; }

        public CommandeController( SignInManager<G_EmployesUser> signInManager, RoleManager<IdentityRole> roleManager,IGestionEmployes<Commande> commandeRepository,IGestionEmployes<Produit> produitRepository,UserManager<G_EmployesUser> userManager, G_EmployesDbContext gestionEmployeContext)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.commandeRepository = commandeRepository;
            this.produitRepository = produitRepository;
            this.userManager = userManager;
            this.gestionEmployeContext = gestionEmployeContext;
         
        }
        //[Authorize(Roles = "chef,admin")]
        // GET: CommandeController
        public async Task<ActionResult> Index()
        {

            //await roleManager.CreateAsync(new IdentityRole("admin"));
            //await roleManager.CreateAsync(new IdentityRole("chef"));
            //await roleManager.CreateAsync(new IdentityRole("overier"));

            if (signInManager.IsSignedIn(User))
            {
                G_EmployesUser user = await userManager.FindByEmailAsync(User.Identity.Name);

                if (user != null)
                {
                    //set sessions ::
                    HttpContext.Session.SetString("IDUser", user.Id);
                    HttpContext.Session.SetInt32("IDEmployeCon", user.IdEmploye);
                    HttpContext.Session.SetString("NomUser", user.Nom);
                    HttpContext.Session.SetString("PrenomUser", user.Prenom);
                    HttpContext.Session.SetString("ImageUser", user.ImageUrl);
                    if (User.IsInRole("chef"))
                    {
                        HttpContext.Session.SetString("role", "Chef de chantier");
                    }
                    else if (User.IsInRole("overier"))
                    {
                        HttpContext.Session.SetString("role", "Overier");
                    }
                    else if (User.IsInRole("admin"))
                    {
                        HttpContext.Session.SetString("role", "Administrateur");
                    }
                }
            }






            foreach (var item in commandeRepository.List())
            {
                if (item.Etat == "P_A")
                {
                    ViewBag.etatcommande = "P_A";
                }
            }


           

            var commandes = commandeRepository.List().ToList();
            return View(commandes);
        }
         [Authorize(Roles = "chef,admin")]

        // GET: CommandeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var commande = commandeRepository.Find(id);
                return View(commande);
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        // GET: CommandeController/Create
        [Authorize(Roles = "chef")]
        public ActionResult Create()
        {
            var model =new CommandeProduitOveierViewModel
            {
                produits = produitRepository.List().ToList(),
               
            };
            return View(model);
        }
        [Authorize(Roles = "chef,admin")]
        // POST: CommandeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommandeProduitOveierViewModel model)
        {
            try
            {
            
                 var email =User.Identity.Name;
                  var overier = new Overier();
                foreach(var item in gestionEmployeContext.overiers)
                {
                    if (item.Email == email) overier = item;
                }
               
                var produit = produitRepository.Find(model.idProduit);
                var commande = new Commande
                {
                    Qtt_Diduir = -model.Qtt_Diduir,
                    Date_operation = DateTime.Now,
                    Etat = "P_A",
                    ovrier = overier,
                    produit = produit
                };
           
                  commandeRepository.Add(commande);
                 
                    return RedirectToAction(nameof(Index));
               
              
            }
            catch
            {
                return View();
            }
        }



        [Authorize(Roles = "chef,admin")]
        public void deleteCommandes(List<int> listid)
        {
            try
            {
                foreach(var item in listid)
                {
                    commandeRepository.Delete(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize(Roles = "chef,admin")]
        public ActionResult CommandeRefuse(int id)
        {
            var commande = commandeRepository.Find(id);
            commande.Etat = "R";
            commandeRepository.Update(id, commande);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "chef,admin")]
        public ActionResult CommandeRefuseAll(List<int> listid)
        {
            foreach(var id in listid)
            {

                    var item = commandeRepository.Find(id);
                    item.Etat = "R";
                    commandeRepository.Update(item.Id, item);
              
            }
           
            return RedirectToAction(nameof(Index));
        }
    }
}
