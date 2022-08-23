using G_Employes.Areas.Identity.Data;
using G_Employes.ViewModel;
using GestionEmployes.Models;
using GestionEmployes.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Employes
{
    public class OverierController : Controller
    {
        private readonly IGestionEmployes<Overier> overierRepository;
        private readonly IGestionEmployes<CategorieOverier> categorieRepository;

        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<G_EmployesUser> userManager;
        private readonly SignInManager<G_EmployesUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        [Obsolete]
        public OverierController(IGestionEmployes<Overier> overierRepository,IGestionEmployes<CategorieOverier> categorieRepository,IHostingEnvironment hosting
                                ,UserManager<G_EmployesUser> userManager,SignInManager<G_EmployesUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.overierRepository = overierRepository;
            this.categorieRepository = categorieRepository;
            this.hosting = hosting;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        // GET: OverierController
        public ActionResult Index()
        {
            var overiers = overierRepository.List();
            return View(overiers);
        }

        // GET: OverierController/Details/5
        public ActionResult Details(int id)
        {
            var overier = overierRepository.Find(id);
            return View(overier);
        }

        // GET: OverierController/Create
        public ActionResult Create()
        {

            return View(GetallCategories());
        }
         public int compteur = 1;
        // POST: OverierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
       
        public async Task<ActionResult> Create(CategorieOverierViewModel model)
        {
            try
            {
               
                string filename = string.Empty;
                if(model.File != null)
                {
                    string Upload = Path.Combine(hosting.WebRootPath, "Uploads");
                    filename = model.File.FileName;
                    string filepath = Path.Combine(Upload, filename);
                    model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                
                
                var OVERIERcategorie = categorieRepository.Find(model.IdCategorie);
                Overier NvOverier = new Overier
                {

                    Name = model.Name,
                    Prenom = model.Prenom,
                    Adress = model.Adress,
                    Sexe = model.Sexe,
                    CIN = model.CIN,
                    Date_embauche = model.Date_embauche,
                    Email = model.Email,
                    Password = model.Password,
                    Tel = model.Tel,
                    Type = model.Type,
                    ImageUrl = filename,
                    categorie = OVERIERcategorie, 
                };


                var Etat = false;
                foreach (var item in overierRepository.List())
                {
                    if (item.Email == model.Email)
                    {
                        Etat = true;
                    }

                }

                if (model.IdCategorie == -1 )
                {
                    ViewBag.Message = "selectionner un Categorie dans la list!";

                    return View(GetallCategories());
                }
                else if(Etat)
                {
                    ViewBag.Message = "Email  deje exist dans la base de donnees";
                    return View(GetallCategories());
                }
                else
                {
                    overierRepository.Add(NvOverier);
                    compteur += 1;


                    //Register user usin managerUser and sgin in user; ; ; ;
                    //var username = GenerateUserName(model.Name, model.Prenom);
                    G_EmployesUser user = new G_EmployesUser { Email = model.Email, UserName = model.Email, Nom = model.Name, Prenom = model.Prenom ,ImageUrl = filename };
                    await userManager.CreateAsync(user, model.Password);
                    //await signInManager.SignInAsync(user,true);
                    ////make a role for this user ::::::::::
                    //var role = await roleManager.FindByNameAsync(model.Type);
                    await userManager.AddToRoleAsync(user, model.Type);

                     return RedirectToAction(nameof(Index));

                }







               
                
            }
            catch
            {
                return View();
            }
        }
       
        //bach ngenerer username onjme3 fih nom m3a lprenom . for generate username it means put the firstname and lastname in username variable;;;;
        private string GenerateUserName(string nom,string prenom)
        {
            return nom.Trim().ToUpper() + "_" + prenom.Trim().ToLower();
        }
        // GET: OverierController/Edit/5
    
        public async Task<ActionResult> Edit(int id)
        {
            var overier = overierRepository.Find(id);
            var categorieid = overier.categorie == null ?0: overier.categorie.Id;
            var user = await userManager.FindByEmailAsync(overier.Email);

            var model = new CategorieOverierViewModel
            {
                OverierId = overier.Id,
                Name = overier.Name,
                Prenom = overier.Prenom,
                Adress = overier.Adress,
                Sexe = overier.Sexe,
                CIN = overier.CIN,
                Date_embauche = overier.Date_embauche,
                Email = overier.Email,
                Password = overier.Password,
                Tel = overier.Tel,
                Type = overier.Type,
                ImageUrl = overier.ImageUrl,
                IdCategorie = categorieid,
                categories = categorieRepository.List().ToList(),
                user = user
            };
            return View(model);
        }

        // POST: OverierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Edit(int id,CategorieOverierViewModel model)
        {
            try
            {
                var roleOld = overierRepository.Find(id).Type;

                var imgUrl = UpatePicture(model);
        
                Overier overier = new Overier
                {
                    Id=id,
                    Name = model.Name,
                    Prenom = model.Prenom,
                    Adress = model.Adress,
                    Sexe = model.Sexe,
                    CIN = model.CIN,
                    Date_embauche = model.Date_embauche,
                    Email = model.Email,
                    Password = model.Password,
                    Tel = model.Tel,
                    Type = model.Type,
                    ImageUrl = imgUrl,
                    categorie =categorieRepository.Find(model.IdCategorie),
                };


                overierRepository.Update(id, overier);




                //Find Old user ; ; ;
                if(model.user!=null)
                {
                    var user = await userManager.FindByIdAsync(model.user.Id);

                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.Nom = model.Name;
                        user.Prenom = model.Prenom;
                        user.UserName = model.Email;
                        user.ImageUrl = imgUrl;
                        ////update password by remove the old and add the new ::::
                        await userManager.RemovePasswordAsync(user);
                        await userManager.AddPasswordAsync(user, model.Password);
                        await userManager.UpdateAsync(user);
                    }

                    //for update a role do remove and add 
                    await userManager.RemoveFromRoleAsync(user,roleOld);
                    await userManager.AddToRoleAsync(user, model.Type);
                }
             
              
               

                     





                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Obsolete]
        private string UpatePicture(CategorieOverierViewModel model)
        {
            string filename = string.Empty;
            string oldFilename = model.ImageUrl;
            if (model.File != null)
            {
                string Upload = Path.Combine(hosting.WebRootPath, "Uploads");
                filename = model.File.FileName ;
                string filepath = Path.Combine(Upload, filename);

                //pour modifier une image il faut le supp
                //Supprimer l'acienne image 

                string fullpathold = oldFilename == null ? null : Path.Combine(Upload, oldFilename);

                if (filepath != fullpathold)
                {
                    if (fullpathold != null)
                    {
                        System.IO.File.Delete(fullpathold);
                        //pour enregistrer l'image
                        model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                    }
                    else
                        model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                }
            }

            if (filename == "") return oldFilename; else return oldFilename;
        }
        // GET: OverierController/Delete/5
        public ActionResult Delete(int id)
        {
            var overier = overierRepository.Find(id);
            return View(overier);
        }

        // POST: OverierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Delete(int id, Overier overier)
        {
            try
            {



                overier = overierRepository.Find(id);
                //find user from the email
                var user = await userManager.FindByEmailAsync(overier.Email);


                //delete employe 
                overierRepository.Delete(id);
                

                //delete image::
                string Upload = Path.Combine(hosting.WebRootPath, "Uploads");
                var filename = overier.ImageUrl;
                if (filename != null)
                {
                     string fullpath = Path.Combine(Upload, filename);
                     System.IO.File.Delete(fullpath);
                }
            
             
                 //delete user that you finded above:::
                 await userManager.DeleteAsync(user);

                //delete user from roles
                DeleteUserRole(user);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async void DeleteUserRole(G_EmployesUser user)
        {
            var roles = new List<string>();

            foreach (var role in roleManager.Roles)
            {
                if (await userManager.IsInRoleAsync(user, role.Name)) roles.Add(role.Name);
            }

            if (roles.Count > 0) await userManager.RemoveFromRolesAsync(user, roles);
        }


        CategorieOverierViewModel GetallCategories()
        {
            var model = new CategorieOverierViewModel
            {
                categories = categorieRepository.List().ToList()
            };

            return model;
        }
    }
}
