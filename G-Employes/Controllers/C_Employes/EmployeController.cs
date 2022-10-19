using ClosedXML.Excel;
using G_Employes.Areas.Identity.Data;
using G_Employes.Data;
using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using GestionEmployes.Models.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers
{
    
    public class EmployeController : Controller
    {
        private readonly G_EmployesDbContext gestionEmployeContext;
        private readonly IGestionEmployes<Commande> commandeRepository;
        [System.Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<G_EmployesUser> userManager;
        private readonly SignInManager<G_EmployesUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        [System.Obsolete]
        public EmployeController(G_EmployesDbContext gestionEmployeContext, IGestionEmployes<Commande> commandeRepository,
            IHostingEnvironment hosting,UserManager<G_EmployesUser> userManager, SignInManager<G_EmployesUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.gestionEmployeContext = gestionEmployeContext;
            this.commandeRepository = commandeRepository;
            this.hosting = hosting;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [Authorize(Roles = "admin")]
        // GET: EmployeController
        public ActionResult Index()
        { 
             var user = gestionEmployeContext.overiers.Include(x=>x.Categorie).Include(x => x.Equipe).Include(x => x.Equipe.Projet).ToList();
             return View(user);
        }
        [Authorize(Roles = "admin")]
        public ActionResult GETDATA()
        {
            var build = new StringBuilder();
            build.AppendLine("Id,Nom,Prenom");
           foreach(var item in gestionEmployeContext.overiers)
            {
                build.AppendLine($"{item.Id},{item.Nom},{item.Prenom}");
            }
            return File(Encoding.UTF8.GetBytes(build.ToString()),"text/csv","employes/csv");
        }
        [Authorize(Roles = "admin")]
        public ActionResult Export()
        {
            using(var workbook  = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employes");
                var cell = 1;
                worksheet.Cell(cell, 1).Value = "#Matricule";
                worksheet.Cell(cell, 2).Value = "Name";
                worksheet.Cell(cell, 3).Value = "Adress";
                worksheet.Cell(cell, 4).Value = "Téléphone";
                worksheet.Cell(cell, 5).Value = "CIN";
                worksheet.Cell(cell, 6).Value = "Email";
                worksheet.Cell(cell, 7).Value = "Salaire";

                foreach (var item in gestionEmployeContext.overiers)
                {
                    cell++;
                    worksheet.Cell(cell, 1).Value = item.Matricule;
                    worksheet.Cell(cell, 2).Value = item.Nom + ' ' + item.Prenom;
                    worksheet.Cell(cell, 3).Value = item.Adress;
                    worksheet.Cell(cell, 4).Value = item.Tel;
                    worksheet.Cell(cell, 5).Value = item.CIN;
                    worksheet.Cell(cell, 6).Value = item.Email;
                    worksheet.Cell(cell, 7).Value = item.Salaire;
                }
                using(var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employes.xls");
                }
            }
        }
        [Authorize(Roles = "admin")]

        // GET: EmployeController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.ListCategories = ListCategories();
            ViewBag.ListEquipe = ListEquipe();
            ViewBag.ListProjet = ListProjet();
            return View(gestionEmployeContext.overiers.Find(id));
        }
        [Authorize(Roles = "chef,overier")]
        
        public ActionResult DetailsEm(int id)
        {
            ViewBag.ListCategories = ListCategories();
            ViewBag.ListEquipe = ListEquipe();
            ViewBag.ListProjet = ListProjet();
            ViewBag.ListProjet = ListEmplye();
            return View(gestionEmployeContext.overiers.Find(id));
        }
        [Authorize(Roles = "admin")]
        List<Categorie> ListCategories()
        {
            var categories = gestionEmployeContext.categorie.ToList();
            categories.Insert(0, new Categorie { Id = -1, Fonctionnalite = "--select fonctionnalite--" });

            return categories;
        }
        List<Overier> ListEmplye()
        {
            var employe = gestionEmployeContext.overiers.ToList();
            employe.Insert(0, new Overier { Id = -1, Nom = "--List Des Employes--" });

            return employe;
        }
        [Authorize(Roles = "admin")]
        List<Projet> ListProjet()
        {
            var projet = gestionEmployeContext.projet.ToList();
           

            return projet;
        }
        [Authorize(Roles = "admin")]
        public List<Equipe> ListEquipe()
        {
            var employe = gestionEmployeContext.equipe.ToList();
            employe.Insert(0, new Equipe { Id = -1, Nom_equipe = "--select Nom de equipe--" });

            return employe;
        }
        
        [Authorize(Roles = "admin")]
        public JsonResult GetProjet(string id)
        {
            List<Projet> projet = new List<Projet>();
              projet = gestionEmployeContext.projet.ToList();
            return Json(projet);
        }
        [Authorize(Roles = "admin")]
        public JsonResult GetEquipe(string id)
        {
            List<Equipe> equipe = new List<Equipe>();
            equipe = gestionEmployeContext.equipe.ToList();
            return Json(equipe);
        }
        [Authorize(Roles = "admin")]

        // Get categorie
        public JsonResult GetCategorie(string id)
        {

            var categorie = gestionEmployeContext.categorie.ToList();
            return Json(categorie);
        }

        [Authorize(Roles = "admin")]

        // GET: EmployeController/Create
        public ActionResult Create()
        {
            ViewBag.ListCategories = ListCategories();
            ViewBag.ListEquipe = ListEquipe();
            return View();
        }


        [Obsolete]
        [Authorize(Roles = "admin")]
        public async Task<JsonResult> CreateZ(Overier employe)
        {
            if (ModelState.IsValid)
         { 
            foreach (var item in gestionEmployeContext.overiers.ToList())
            {

                if (item.Email == employe.Email) return Json("Email deja existe");
                else if (item.CIN == employe.CIN) return Json("CIN deja existe");
                else if (item.Tel == employe.Tel) return Json("téléphone deja existe");


            }

            string newFileName = "default1.jpg";


            if (employe.File != null)
            {
                string upload = Path.Combine(hosting.WebRootPath, "uploads");
                string extension = Path.GetExtension(employe.File.FileName);


                do
                {
                    newFileName = GenereFilename().Trim() + extension.ToLower();
                }
                while (System.IO.File.Exists(newFileName));


                string filepath = Path.Combine(upload, newFileName);
                employe.File.CopyTo(new FileStream(filepath, FileMode.Create));
            }






            var categorie = gestionEmployeContext.categorie.Find(employe.Categorie.Id);
            var equipe = gestionEmployeContext.equipe.Find(employe.Equipe.Id);
            var newEploye = new Overier
            {
                Nom = employe.Nom,
                Prenom = employe.Prenom,
                Adress = employe.Adress,
                Tel = employe.Tel,
                Date_embauche = employe.Date_embauche,
                CIN = employe.CIN,
                Sexe = employe.Sexe,
                Type = employe.Type,
                Email = employe.Email,
                Password = employe.Password,
                Salaire = employe.Salaire,
                Image = newFileName,
                EmailOld = employe.Email,
                typeOld = employe.typeOld

            };
                if(categorie != null)
                {
                    newEploye.Categorie = categorie;

                }
                if (equipe != null)
                {
                    newEploye.Equipe = equipe;
                }


                gestionEmployeContext.overiers.Add(newEploye);
            gestionEmployeContext.SaveChanges();
            matricule(newEploye);
            //SendMail(newEploye);

            //Register user usin managerUser and sgin in user; ; ; ;
            //var username = GenerateUserName(model.Name, model.Prenom);
            G_EmployesUser user = new G_EmployesUser { Email = employe.Email, UserName = employe.Email, Nom = employe.Nom, Prenom = employe.Prenom, ImageUrl = newFileName, IdEmploye = newEploye.Id };
            await userManager.CreateAsync(user, employe.Password);
            //await signInManager.SignInAsync(user,true);
            ////make a role for this user :::::::::: ////make a role for this user ::::::::::
            //var role = await roleManager.FindByNameAsync(model.Type);
            await userManager.AddToRoleAsync(user, employe.Type);


            return Json("done");

        }
            else
            {
                List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(errors[0]);

            }

        }
        [Authorize(Roles = "admin")]

        public void SendMail(Overier overier)
        {
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("tuna-vr@outlook.com", "testtuna123@");
            MailMessage msg = new MailMessage("tuna-vr@outlook.com", overier.Email );
            msg.Subject = "Tuna Verdure";
            msg.IsBodyHtml = true;
            msg.Body = "<h3>Bonjour : " + overier.Nom.ToUpper()+" "+overier.Prenom + "</h3><br><h4>Votre adresse e-mail et mot de passe pour accéder au site Tuna Verdure</h4>" + "<ul><li> Adresse E-mail  : "+overier.Email+ "</li></br><li> Mot De Passe : "+overier.Password+"</li></ul>";
            smtp.Send(msg);

        }
        [Authorize(Roles = "admin")]
        private void matricule(Overier ov)
        {
            ov.Matricule = ov.Type.Substring(0, 2).ToUpper() + ov.Id;
            gestionEmployeContext.overiers.Update(ov);
            gestionEmployeContext.SaveChanges();

        }
        [Authorize(Roles = "admin")]
        //generate name image
        public string GenereFilename()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var newFileName = new String(stringChars);
            return newFileName;
        }
        [Authorize(Roles = "admin")]
        public JsonResult addEquipe(string nom)
        {
            if (nom != null)
            {
                foreach (var item in gestionEmployeContext.equipe.ToList())
                {
                    if (item.Nom_equipe == nom) return Json("equipe deja existe");
                }
                var Newequipe = new Equipe()
                {
                    Nom_equipe = nom,

                };
                gestionEmployeContext.equipe.Add(Newequipe);
                gestionEmployeContext.SaveChanges();
                return Json(Newequipe);
            }
            else
            {
                return Json("input empty");
            }


        }
        [Authorize(Roles = "admin")]
        public JsonResult addEqPr(Equipe equipe ,Projet projet)

        {

            var equipee = gestionEmployeContext.equipe.Find(equipe.Id);
            var projett = gestionEmployeContext.projet.Find(projet.Id);

            equipee.Projet = projett;
            gestionEmployeContext.equipe.Update(equipee);
            gestionEmployeContext.SaveChanges();

            return Json("done");
        }
        [Authorize(Roles = "admin")]
        public JsonResult addProjet(string nomP,DateTime dateD ,DateTime dateF,string etat )
        {

            if (nomP != null)
            {

                var Newprojet = new Projet()
                {
                    Nom_projet = nomP,
                    Date_debut = dateD,
                    Date_fin = dateF,
                    Etat = etat
                };
                gestionEmployeContext.projet.Add(Newprojet);
                gestionEmployeContext.SaveChanges();
                return Json(Newprojet);
            }
            else
            {
                return Json("input empty");
            }


        }
        [Authorize(Roles = "admin")]
        // GET: EmployeController/Edit/5
        public ActionResult Edit(int id)
        {
            var emlp = gestionEmployeContext.overiers.Find(id);

            ViewBag.ListProjet = ListProjet();
            ViewBag.ListCategories = ListCategories();
            ViewBag.ListEquipe = ListEquipe();
            var overier = gestionEmployeContext.overiers.Find(id);
       
            return View(overier);
        }

        [Authorize(Roles = "admin")]
        [Obsolete]
        public async Task<JsonResult> Editz(Overier employe)
        {
            string newFileName = string.Empty, newName = GenereFilename();
            string oldFilename = employe.Image;
            if (employe.File != null)
            {
                string Upload = Path.Combine(hosting.WebRootPath, "uploads");

                string extension = Path.GetExtension(employe.File.FileName);
                newFileName = newName.Trim() + extension.ToLower();
                string filepath = Path.Combine(Upload, newFileName);
                //pour modifier une image il faut le supp
                //Supprimer l'acienne image 

                string fullpathold = oldFilename == null ? null : Path.Combine(Upload, oldFilename);

                if (filepath != fullpathold)
                {
                    if (fullpathold != null  )
                    {
                        if(oldFilename != "default1.jpg")
                        {
                           
                            System.IO.File.Delete(fullpathold);

                        }
                        //pour enregistrer l'image
                        employe.File.CopyTo(new FileStream(filepath, FileMode.Create));
                    }
                    else 
                    { 
                        employe.File.CopyTo(new FileStream(filepath, FileMode.Create));
                    }
                    var categorie = gestionEmployeContext.categorie.Find(employe.Categorie.Id);
                    var equipe = gestionEmployeContext.equipe.Find(employe.Equipe.Id);
                    var oldemp = gestionEmployeContext.overiers.AsNoTracking().SingleOrDefault(p => p.Id == employe.Id);

                    var emp = new Overier
                    {
                        Id = employe.Id,
                        Nom = employe.Nom,
                        Prenom = employe.Prenom,
                        Adress = employe.Adress,
                        Tel = employe.Tel,
                        Date_embauche = employe.Date_embauche,
                        CIN = employe.CIN,
                        Sexe = employe.Sexe,
                        Type = employe.Type,
                        Email = employe.Email,
                        Password = employe.Password,
                        Salaire = employe.Salaire,
                       Matricule=oldemp.Matricule,
                        Image = newFileName,
                        Categorie = categorie,
                        Equipe = equipe,
                        EmailOld = employe.Email,
                        typeOld = employe.typeOld

                    };

                    






                    //Find Old user ; ; ;
                    var user = await userManager.FindByEmailAsync(employe.EmailOld);

                    gestionEmployeContext.overiers.Update(emp);

                    


                    if (user != null)
                        {
                            user.Email = employe.Email;
                            user.Nom = employe.Nom;
                            user.Prenom = employe.Prenom;
                            user.UserName = employe.Email;
                            user.ImageUrl = newFileName;
                            ////update password by remove the old and add the new ::::
                            await userManager.RemovePasswordAsync(user);
                            await userManager.AddPasswordAsync(user, employe.Password);
                            await userManager.UpdateAsync(user);



                        //for update a role do remove and add 
                        await userManager.RemoveFromRoleAsync(user, employe.typeOld);
                        await userManager.AddToRoleAsync(user, employe.Type);
                    }

                    gestionEmployeContext.SaveChanges();
                    //matricule(emp);


                }
            }
            else { 

                //string fileName = UploadFile(employe.File, employe.Image);
                var categorie = gestionEmployeContext.categorie.Find(employe.Categorie.Id);
                var equipe = gestionEmployeContext.equipe.Find(employe.Equipe.Id);
                var oldemp = gestionEmployeContext.overiers.AsNoTracking().SingleOrDefault(p => p.Id == employe.Id);


                var emp = new Overier
               {
                   Id = employe.Id,
                   Nom = employe.Nom,
                   Prenom = employe.Prenom,
                   Adress = employe.Adress,
                   Tel = employe.Tel,
                   Date_embauche = employe.Date_embauche,
                   CIN = employe.CIN,
                   Sexe = employe.Sexe,
                   Type = employe.Type,
                   Email = employe.Email,
                   Password = employe.Password,
                   Salaire = employe.Salaire,
                   Matricule= oldemp.Matricule,
                   Image = employe.Image,
                   Categorie = categorie,
                   Equipe = equipe,
                   EmailOld = employe.Email,
                   typeOld = employe.typeOld
               };

                
                var user = await userManager.FindByEmailAsync(employe.EmailOld);
                gestionEmployeContext.overiers.Update(emp);
               

                //Find Old user ; ; ;
                


                if (user != null)
                {
                    user.Email = employe.Email;
                    user.Nom = employe.Nom;
                    user.Prenom = employe.Prenom;
                    user.UserName = employe.Email;
                    user.ImageUrl = oldFilename;
                    ////update password by remove the old and add the new ::::
                    await userManager.RemovePasswordAsync(user);
                    await userManager.AddPasswordAsync(user, employe.Password);
                    await userManager.UpdateAsync(user);



                    //for update a role do remove and add 
                    await userManager.RemoveFromRoleAsync(user, employe.typeOld);
                    await userManager.AddToRoleAsync(user, employe.Type);
                }
                gestionEmployeContext.SaveChanges();
                //matricule(emp);
            }
            return Json("done");
        }
        [Authorize(Roles = "admin")]

        [Obsolete]
        public void DeletImg(string ovr)
        {
            string img = ovr;
            string Upload = Path.Combine(hosting.WebRootPath, "uploads");
            string fullpath = Path.Combine(Upload, img);
            if (System.IO.File.Exists(fullpath))
            {
                if (img != "default1.jpg")
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(fullpath);


                }
            }
        }
        [Authorize(Roles = "admin")]
        // GET: EmployeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(gestionEmployeContext.overiers.Find(id));
        }
        [Authorize(Roles = "admin")]
        // POST: EmployeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
           

                var overier = gestionEmployeContext.overiers.Find(id);
            

            foreach (var item in gestionEmployeContext.equipe)
                {
                    if(item.Employes == overier)
                    {
                       gestionEmployeContext.Remove(item);
                        //ViewBag.MessageEquipe = "c'est employe deja dans une equipe";
                    }
                }

            //find user from the email
            var user = await userManager.FindByEmailAsync(overier.Email);
            //delete employe 

            CheckcommandeoperationRelation(id);
          
            gestionEmployeContext.overiers.Remove(overier);
          
            gestionEmployeContext.SaveChanges();
            DeletImg(overier.Image);

            if (user!=null)
            {
                //delete user that you finded above:::
                await userManager.DeleteAsync(user);

                //delete user from roles
                var roles = new List<string>();

                foreach (var role in roleManager.Roles)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name)) roles.Add(role.Name);
                }

                if (roles.Count > 0) await userManager.RemoveFromRolesAsync(user, roles);
            }
           


            return RedirectToAction(nameof(Index));


        }

        [Authorize(Roles = "admin")]
        public void CheckcommandeoperationRelation(int id)
        {
            var overier = gestionEmployeContext.overiers.Find(id);
            //var nproduit = new Produit { Desgination = produit.Desgination, Prix = produit.Prix, ImageUrl = produit.ImageUrl, Categorie = produit.Categorie, Quantite = produit.Quantite };

            foreach (var item in commandeRepository.List())
            {
                if (item.ovrier == overier)
                {

                    commandeRepository.Delete(item.Id);
                    //item.produit = nproduit;

                }

            }


        }
    }
}
